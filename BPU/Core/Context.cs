using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class Context : IVariableContainer
    {
        public Guid ContextId;
        public ProcessingStatus Status;
        public string StatusMessage;

        public Dictionary<string, object> Variables { get; private set; }
        public List<Scope> Scopes { get; set; }

        public Host Host;

        public HashSet<Guid> RunningScopeIds;


        public Context()
        {
            RunningScopeIds = new HashSet<Guid>();
        }


        public static T Spawn<T>() where T : Context, new()
        {
            return new T()
            {
                ContextId = Guid.NewGuid(),
                Status = ProcessingStatus.Ready,
                StatusMessage = "Ready.",
                Variables = new Dictionary<string, object>(),
                Scopes = new List<Scope>()
            };
        }
        

        public void SetStatus(ProcessingStatus status, string message, params object[] prms)
        {
            Status = status;
            StatusMessage = prms.Any() ? string.Format(message, prms) : message;
            DoUpdate();
            DoLog(null, StatusMessage);
        }


        public void DoLog(Guid? scopeId, string message)
        {
            Host.SaveLog(ContextId, scopeId, message);
        }


        public void DoUpdate()
        {
            Host.SaveContext(this);
        }


        public virtual void ScopeUpdated(Scope scope)
        {
        }


        public void Execute()
        {
            var scopesWithIssues =
                Scopes.Where(s => (s.Status == ProcessingStatus.Error
                                || s.Status == ProcessingStatus.Halted)
                                && (s.RetryCount <= 0))
                .ToArray();

            if (scopesWithIssues.Any())
            {
                var scopesList = string.Join(
                    ", ",
                    scopesWithIssues.Select(s => $"ScopeId:{s.ScopeId} Step:{s.CurrentStep?.UniqueName ?? "NULL"} Error:{s.LastError}"));

                throw new InvalidOperationException(
                    $"Önce sorunlu scope kayıtları düzeltilmeli! ({ scopesList })");
            }

            SetStatus(ProcessingStatus.Running, "Context running.");

            var tasks =
                Scopes.Where(s => s.Status == ProcessingStatus.Running)
                .Select(s =>
                    Task.Run(() =>
                    {
                        if (!SetScopeRunning(s.ScopeId, true))
                            return;

                        try { s.Execute(); }
                        catch (Exception ex) { s.LastError = ex; s.SetStatus(ProcessingStatus.Error, $"Error: {ex.Message}"); }
                        finally { SetScopeRunning(s.ScopeId, false); }
                    }))
                .ToArray();

            Task.WaitAll(tasks);

            if (Scopes.Any(s => s.Status == ProcessingStatus.Error))
                SetStatus(ProcessingStatus.Error, "At least one scope is failed!");

            else if (Scopes.Any(s => s.Status == ProcessingStatus.Halted))
                SetStatus(ProcessingStatus.Error, "At least one scope is halted!");

            else if (!Scopes.Any(s => s.Status == ProcessingStatus.Running))
                SetStatus(ProcessingStatus.Finished, "Context finished.");
        }


        public void AddScope(Scope scope)
        {
            Scopes.Add(scope);
            scope.DoUpdate();
        }


        public bool SetScopeRunning(Guid scopeId, bool running)
        {
            if (running == RunningScopeIds.Contains(scopeId))
                return false;

            lock (RunningScopeIds)
            {
                if (running == RunningScopeIds.Contains(scopeId))
                    return false;

                if (running)
                    RunningScopeIds.Remove(scopeId);
                else
                    RunningScopeIds.Add(scopeId);

                return true;
            }
        }
    }
}

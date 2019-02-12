using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class ProcessStep
    {
        public Process Process;

        public Guid ProcessStepId;
        public string Name;

        public ProcessStep NextStep;
        public ProcessStep ErrorStep;

        public List<string> ImportedVariables;
        public List<string> ExportedVariables;


        static int NameCounter = 0;
            

        public ProcessStep()
        {
            ProcessStepId = Guid.NewGuid();
            Name = $"Process Step {++NameCounter}";
        }


        public string UniqueName
        {
            get { return $"{Process.Name}/{Name}"; }
        }


        protected virtual ProcessStep OnExecution(Scope scope)
        {
            return NextStep;
        }


        public ProcessStep Execute(Scope scope)
        {
            scope.SetStatus(ProcessingStatus.Running, "Running.");

            scope.DoLog($"Step {Name} entered.");

            try
            {
                var result = OnExecution(scope);

                if (result == null)
                    scope.SetStatus(ProcessingStatus.Finished, "Finished.");

                return result;
            }
            catch (Exception ex)
            {
                scope.LastError = ex;

                scope.DoLog($"Error while executing {Name} step. Details: {ex.Message}");

                if (ErrorStep == null)
                {
                    scope.SetStatus(ProcessingStatus.Error, $"Error: {ex.Message}");
                }
                
                return ErrorStep;
            }
            finally
            {
                scope.DoLog($"Step {Name} left.");
            }
        }
    }
}

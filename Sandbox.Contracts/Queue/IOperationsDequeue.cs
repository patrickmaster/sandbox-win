using System.Collections.Generic;
using Sandbox.Contracts.Types;
using Sandbox.Contracts.Types.Environment;

namespace Sandbox.Contracts.Queue
{
    /// <summary>
    /// To be used by the environment mechanism
    /// </summary>
    public interface IOperationsDequeue
    {
        /// <summary>
        /// Gets all the unresolved operation requests from the beginning
        /// of the queue
        /// </summary>
        /// <returns>A collection of operation requests</returns>
        IEnumerable<EnvironmentInput> GetUnresolved();

        /// <summary>
        /// Gets first <paramref name="count"/> unresolved operation requests
        /// from the beginning of the queue
        /// </summary>
        /// <param name="count">The number of requests to get</param>
        /// <returns>A collection of operation requests</returns>
        IEnumerable<EnvironmentInput> GetUnresolved(int count);

        /// <summary>
        /// Puts the result of the operation to the request
        /// and marks it as resolved
        /// </summary>
        /// <param name="request">Processed operation request</param>
        /// <param name="result">Operation result</param>
        void Resolve(EnvironmentInput request, EnvironmentOutput result);
    }
}
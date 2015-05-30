using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts.Types;

namespace Sandbox.Contracts.DataLayer
{
    /// <summary>
    /// Inteface to be used by front-end endpoints, e.g. web app
    /// </summary>
    public interface IOperationsQueue
    {
        /// <summary>
        /// Enqueues an operation request
        /// </summary>
        /// <param name="input">Operation request data</param>
        /// <returns>Unique identifier of the request</returns>
        Guid Enqueue(EnvironmentInput input);

        /// <summary>
        /// Tries to obtain the result of an operation equeued with <see cref="Enqueue"/>
        /// method
        /// </summary>
        /// <param name="id">Unique identifier of the operation requested</param>
        /// <param name="output">Operation result or null if unavailable</param>
        /// <returns>True if the result is available, false otherwise</returns>
        bool TryGet(Guid id, out EnvironmentOutput output);
    }
}

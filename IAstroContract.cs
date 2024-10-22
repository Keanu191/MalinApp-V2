/*
 * 30074191 / Keanu Farro
 * 8/10/2024
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel; // import System.ServiceModel as we want to use a ServiceContract
namespace MalinAstroAPP
{
    [ServiceContract]
    internal interface IAstroContract
    {
        /*
         * 7.1 Create the ServiceContract called “IAstroContract.cs” 
         * which will need to be identical to the server without a reference
         * to the AstroMath.DLL.
         * 
         * Implement four operation contracts per each calculation in the astromath DLL
         */
       
        // temperature conversion
        [OperationContract]
        double tempConversion(double tempInCeslius);

        // star velocity
        [OperationContract]
        double measureVolicity(double observedWaveLength, double restWaveLength);

        // star distance
        [OperationContract]
        double calculateStarDistance(double arcSecondsAngle);

        // event horizon
        [OperationContract]
        double calculateEventHorizon(double blackHoleMass);

        [OperationContract]
        bool Ping(); // Adding a simple method to check connectivity
    }
}

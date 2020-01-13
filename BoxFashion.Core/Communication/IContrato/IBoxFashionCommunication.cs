using BoxFashion.Core.Entidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BoxFashion.Core.Communication.IContrato
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IBoxFashionCommunication
    {
        [OperationContract]
        Collection<City> GetCity();

        [OperationContract]
        int InsertLoginCustomer(Security_LoginCustomer oLogin);

        [OperationContract]
        int InsertLoginPartner(Security_LoginPartner oLogin);
    }
 }

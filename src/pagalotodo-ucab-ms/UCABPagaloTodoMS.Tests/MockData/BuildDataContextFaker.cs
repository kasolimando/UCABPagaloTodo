using Bogus;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.TeleTrust;
using System.Text;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;
using static UCABPagaloTodoMS.Core.Entities.ServicioEntity;

namespace UCABPagaloTodoMS.Tests.MockData
{
    public static class BuildDataContextFaker
    {
        //ADMINISTRADOR
        /// <summary>
        ///    Build a AdminsRequest with random values
        /// </summary>
        /// <returns>Returns an AdminsRequest with all their values</returns>
        /// 
        public static AdminsRequest BuildAdminRequest()
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            //New Random AdminsRequest with random values
            var faker = new Faker<AdminsRequest>()
                .RuleFor(a => a.Clave, f => f.Internet.Password())
                .RuleFor(a => a.Correo, f => f.Internet.Email())
                .RuleFor(a => a.DocIdentidad, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.TipoVj, f => f.PickRandom(new[] { "v", "j", "g", "p", "e" }))
                .RuleFor(a => a.Direccion, f => f.Address.FullAddress())
                .RuleFor(a => a.Nombre, f => f.Name.FirstName())
                .RuleFor(a => a.Apellido, f => f.Name.LastName());
            return faker.Generate();
        }
        /// <summary>
        ///    Build a AdminsRequest with random values except the email
        ///    ##Parameters
        ///         - _email: the especific email for the AdminsRequest
        /// </summary>
        /// <returns>Returns an AdminsRequest with all their values</returns>
        ///
        public static AdminsRequest BuildAdminRequestWithEspecificEmail(string email)
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            //New Random AdminsRequest with random values
            var faker = new Faker<AdminsRequest>()
                .RuleFor(a => a.Clave, f => f.Internet.Password())
                .RuleFor(a => a.Correo, email)
                .RuleFor(a => a.DocIdentidad, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.TipoVj, f => f.PickRandom(new[] { "v", "j", "g", "p", "e" }))
                .RuleFor(a => a.Direccion, f => f.Address.FullAddress())
                .RuleFor(a => a.Nombre, f => f.Name.FirstName())
                .RuleFor(a => a.Apellido, f => f.Name.LastName());

            return faker.Generate();
        }


        //PRESTADOR
        /// <summary>
        ///    Build a PrestadorRequest with random values
        /// </summary>
        /// <returns>Returns an PrestadorRequest with all their values</returns>
        ///
        public static PrestadorRequest BuildPrestadorRequest()
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            //New Random PrestadorRequest with random values
            var faker = new Faker<PrestadorRequest>()
                .RuleFor(a => a.Username, f => f.Random.String2(10))
                .RuleFor(a => a.Clave, f => f.Internet.Password())
                .RuleFor(a => a.Correo, f => f.Internet.Email())
                .RuleFor(a => a.DocIdentidad, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.TipoVj, f => f.PickRandom(new[] { "v", "j", "g", "p", "e" }))
                .RuleFor(a => a.Direccion, f => f.Address.FullAddress())
                .RuleFor(a => a.Nombre, f => f.Name.FirstName())
                .RuleFor(a => a.Apellido, f => f.Name.LastName())
                .RuleFor(a => a.Estatus, f => f.Random.Bool());

            return faker.Generate();
        }

        /// <summary>
        ///    Build a PrestadorRequest with random values except the email and Username
        ///    ##Parameters
        ///         - _email: the especific email and username for the PrestadorRequest
        /// </summary>
        /// <returns>Returns an PrestadorRequest with all their values</returns>
        ///

        public static PrestadorRequest BuildPrestadorRequestWithEspecificEmailAndUsername(string email, string username)
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            //New Random PrestadorRequest with random values
            var faker = new Faker<PrestadorRequest>()
                .RuleFor(a => a.Username, username)
                .RuleFor(a => a.Clave, f => f.Internet.Password())
                .RuleFor(a => a.Correo, email)
                .RuleFor(a => a.DocIdentidad, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.TipoVj, f => f.PickRandom(new[] { "v", "j", "g", "p", "e" }))
                .RuleFor(a => a.Direccion, f => f.Address.FullAddress())
                .RuleFor(a => a.Nombre, f => f.Name.FirstName())
                .RuleFor(a => a.Apellido, f => f.Name.LastName())
                .RuleFor(a => a.Estatus, f => f.Random.Bool());

            return faker.Generate();
        }

        //Consumidor

        /// <summary>
        ///    Build a ConsumidorRequest with random values
        /// </summary>
        /// <returns>Returns an ConsumidorRequest with all their values</returns>
        ///
        public static ConsumidorRequest BuildConsumidorRequest()
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            //New Random ConsumidorRequest with random values
            var faker = new Faker<ConsumidorRequest>()
                .RuleFor(a => a.Username, f => f.Random.String2(10))
                .RuleFor(a => a.Clave, f => f.Internet.Password())
                .RuleFor(a => a.Correo, f => f.Internet.Email())
                .RuleFor(a => a.DocIdentidad, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.TipoVj, f => f.PickRandom(new[] { "v", "j", "g", "p", "e" }))
                .RuleFor(a => a.Direccion, f => f.Address.FullAddress())
                .RuleFor(a => a.Nombre, f => f.Name.FirstName())
                .RuleFor(a => a.Apellido, f => f.Name.LastName())
                .RuleFor(a => a.Estatus, f => f.Random.Bool());

            return faker.Generate();
        }

        /// <summary>
        ///    Build a ConsumidorRequest with random values except the email and Username
        ///    ##Parameters
        ///         - _email: the especific email and Username for the ConsumidorRequest
        /// </summary>
        /// <returns>Returns an ConsumidorRequest with all their values</returns>
        ///

        public static ConsumidorRequest BuildConsumidorRequestWithEspecificEmailAndUsername(string email, string username)
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            //New Random ConsumidorRequest with random values
            var faker = new Faker<ConsumidorRequest>()
                .RuleFor(a => a.Username, username)
                .RuleFor(a => a.Clave, f => f.Internet.Password())
                .RuleFor(a => a.Correo, email)
                .RuleFor(a => a.DocIdentidad, f => f.Random.AlphaNumeric(10))
                .RuleFor(a => a.TipoVj, f => f.PickRandom(new[] { "v", "j", "g", "p", "e" }))
                .RuleFor(a => a.Direccion, f => f.Address.FullAddress())
                .RuleFor(a => a.Nombre, f => f.Name.FirstName())
                .RuleFor(a => a.Apellido, f => f.Name.LastName())
                .RuleFor(a => a.Estatus, f => f.Random.Bool());

            return faker.Generate();
        }

        //SERVICIOS
        /// <summary>
        ///    Build a ServicioRequest with random values
        /// </summary>
        /// <returns>Returns an ServicioRequest with all their values</returns>
        /// 
        public static ServicioRequest BuildServicioRequest()
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            //New Random ServicioRequest with random values
            var servicioFaker = new Faker<ServicioRequest>()
                .RuleFor(s => s.Nombre, f => f.Commerce.ProductName())
                .RuleFor(s => s.Descripcion, f => f.Lorem.Sentence())
                .RuleFor(s => s.Estatus, Status.activo.ToString())
                .RuleFor(s => s.Categoria, f => f.Commerce.Categories(1)[0])
                .RuleFor(s => s.TipoPago, f => f.PickRandom(new[] { "confirmacion", "contado" }))
                .RuleFor(s => s.PrestadorEntityId, f => f.Random.Guid().ToString());

            return servicioFaker.Generate();
        }

        /// <summary>
        ///    Build a ServicioRequest with random values with and especific name
        /// </summary>
        /// <returns>Returns an ServicioRequest with all their values</returns>
        /// 
        public static ServicioRequest BuildServicioRequestWithAnEspecificName(string name, string user)
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            //New Random ServicioRequest with random values
            var servicioFaker = new Faker<ServicioRequest>()
                .RuleFor(s => s.Nombre, name)
                .RuleFor(s => s.Descripcion, f => f.Lorem.Sentence())
                .RuleFor(s => s.Estatus, Status.activo.ToString())
                .RuleFor(s => s.Categoria, f => f.Commerce.Categories(1)[0])
                .RuleFor(s => s.TipoPago, f => f.PickRandom(new[] { "confirmacion", "contado" }))
                .RuleFor(s => s.PrestadorEntityId, user);

            return servicioFaker.Generate();
        }

        //FORMATOS
        /// <summary>
        ///    Build a FormatosRequestFormatosRequest with random values
        /// </summary>
        /// <returns>Returns an ServicioRequest with all their values</returns>
        /// 
        public static FormatosRequest BuildFormatosRequest()
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            var campos = new List<string>();
            campos.Add("Nombre");
            var longitud = new List<int>();
            longitud.Add(10);
            //New Random FormatosRequest with random values
            var servicioFaker = new Faker<FormatosRequest>()
                .RuleFor(t => t.Campos, campos)
                .RuleFor(t => t.Longitud, longitud)
                .RuleFor(t => t.Servicio, f => f.Random.String())
                .RuleFor(t => t.Requerido, f => f.Random.Bool());

            return servicioFaker.Generate();
        }

        /// <summary>
        ///    Build a FormatosRequestFormatosRequest with random values with especific servicio
        /// </summary>
        /// <returns>Returns an ServicioRequest with all their values</returns>
        /// 
       public static FormatosRequest BuildFormatosRequestWithEspecifiServicio(string servicio)
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            var campos = new List<string>();
            campos.Add("Nombre");
            var longitud = new List<int>();
            longitud.Add(10);
            //New Random FormatosRequest with random values
            var servicioFaker = new Faker<FormatosRequest>()
                .RuleFor(t => t.Campos, campos)
                .RuleFor(t => t.Longitud, longitud)
                .RuleFor(t => t.Servicio, servicio)
                .RuleFor(t => t.Requerido, f => f.Random.Bool());

            return servicioFaker.Generate();
        }

        //STATUS

        // <summary>
        //    Build a StatusUserRequest with random values
        // </summary>
       // <returns>Returns an StatusUserRequest with all their values</returns>

        public static StatusUserRequest BuildStatusUserRequest()
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            //New Random StatusUserRequest with random values
            var statusFaker = new Faker<StatusUserRequest>()
                .RuleFor(u => u.Estatus, f => f.Random.Bool());

            return statusFaker.Generate();
        }

        // <summary>
        //    Build a StatusUserRequest with random values
        // </summary>
        // <returns>Returns an StatusUserRequest with all their values</returns>

        public static StatusUserRequest BuildStatusUserRequestEspecific(bool status)
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            //New Random StatusUserRequest with random values
            var statusFaker = new Faker<StatusUserRequest>()
                .RuleFor(u => u.Estatus, status);

            return statusFaker.Generate();
        }

        ///CAMBIO DE CLAVE
        /// <summary>
        ///    Build a CambioClaveUserRequest with random values
        /// </summary>
        /// <returns>Returns an CambioClaveUserRequest with all their values</returns>
        /// 
        public static CambioClaveUserRequest BuildCambioClaveUserRequestWithSpecificInfo(string clave)
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            //New Random StatusUserRequest with random values
            var servicioFaker = new Faker<CambioClaveUserRequest>()
                .RuleFor(u => u.Clave_actual,clave)
                .RuleFor(u => u.Clave_nueva, u => u.Internet.Password());

            return servicioFaker.Generate();
        }

        //STATUS SERVICIO REQUEST
        /// <summary>
        ///    Build a StatusServicioRequest with random values
        /// </summary>
        /// <returns>Returns an StatusServicioRequest with all their values</returns>
        /// 
        public static StatusServicioRequest BuildStatusServicioRequest()
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            //New Random StatusUserRequest with random values
            var servicioFaker = new Faker<StatusServicioRequest>()
                .RuleFor(u => u.Estatus, "1");

            return servicioFaker.Generate();
        }

        public static GuardarConciliacionRequest BuildConciliacionRequest()
        {
            var servicioFaker = new Faker<GuardarConciliacionRequest>()
                 .RuleFor(u => u.Servicio, "servicio3")
                 .RuleFor(t => t.PagoId, "a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a80")
                 .RuleFor(u => u.Aceptado, "1");

            return servicioFaker.Generate();

        }

        ///DEUDA REQUEST
        /// <summary>
        ///    Build a StatusServicioRequest with random values
        /// </summary>
        /// <returns>Returns an StatusServicioRequest with all their values</returns>
        /// 

        public static GuardarDeudaRequest BuildGuardarDeudaRequest()
        {
            var servicioFaker = new Faker<GuardarDeudaRequest>()
                 .RuleFor(u => u.Servicio, "servicio3")
                 .RuleFor(t => t.Username, "username4")
                 .RuleFor(u => u.Monto, "90");

            return servicioFaker.Generate();

        }

        /// <summary>
        ///    Build a StatusServicioRequest with random values
        /// </summary>
        /// <returns>Returns an StatusServicioRequest with all their values</returns>
        /// 

        public static GuardarDeudaRequest BuildGuardarDeudaRequestBadMonto(string username,string monto)
        {
            var servicioFaker = new Faker<GuardarDeudaRequest>()
                 .RuleFor(u => u.Servicio, "servicio3")
                 .RuleFor(t => t.Username, username)
                 .RuleFor(u => u.Monto, monto);

            return servicioFaker.Generate();

        }

        public static IFormFile BuildArchivo()
        {
            var conciliacion = new FormFile(
                                             baseStream: new MemoryStream(Encoding.UTF8.GetBytes("This is a test file")),
                                             baseStreamOffset: 0,
                                             length: 100,
                                             name: "testFile",
                                             fileName: "testFile.txt"
                                         );
            conciliacion.Headers = new HeaderDictionary();
            conciliacion.Headers.Add("Content-Type", "text/plain");
            return conciliacion;
        }

        /// <summary>
        ///    Build a StatusServicioRequest with random values
        /// </summary>
        /// <returns>Returns an StatusServicioRequest with all their values</returns>
        /// 
        public static IFormFile BuildArchivoWithEmptyName()
        {
            var conciliacion = new FormFile(
                                            baseStream: new MemoryStream(Encoding.UTF8.GetBytes("This is a test file")),
                                            baseStreamOffset: 0,
                                            length: 100,
                                            name: "testFile",
                                            fileName: string.Empty
                                            );
            return conciliacion;
        }

        ///PAGO
        /// <summary>
        ///    Build a StatusServicioRequest with random values
        /// </summary>
        /// <returns>Returns an StatusServicioRequest with all their values</returns>
        /// 
        public static PagoRequest BuildPagoRequestWithEspecificData(string servicio, string consumidor)
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            //New Random StatusUserRequest with random values
            var servicioFaker = new Faker<PagoRequest>()
                .RuleFor(u => u.Servicio, servicio)
                .RuleFor(t => t.Monto, f => f.Random.Double(10, 100))
                .RuleFor(u => u.Consumidor, consumidor);

            return servicioFaker.Generate();
        }

        /// <summary>
        ///    Build a StatusServicioRequest with random values
        /// </summary>
        /// <returns>Returns an StatusServicioRequest with all their values</returns>
        /// 
        public static PagoRequest BuildPagoRequest()
        {
            //Seed of the random number generator used by the Faker
            Randomizer.Seed = new Random(100);
            //New Random StatusUserRequest with random values
            var servicioFaker = new Faker<PagoRequest>()
                .RuleFor(u => u.Servicio, u => u.Commerce.ProductName())
                .RuleFor(t => t.Monto, f => f.Random.Double(10, 100))
                .RuleFor(u => u.Consumidor, f => f.Person.UserName);

            return servicioFaker.Generate();
        }

        public static PagoEntity BuildPago()
        {
            var pago = new PagoEntity
            {
                Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a80"),
                Monto = 20.5,
                Fecha = DateTime.Now,
                Aprobado = false,
                Cierre = true,
                ServicioEntityId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13"),
                ConsumidorEntityId = "username5",
                //FechaCierre = "servicio1 20230515",
                Consumidor = new ConsumidorEntity
                {
                    Username = "username5",
                    Clave = "username5",
                    Correo = "username5@gmail.com",
                    DocIdentidad = "5678",
                    TipoVj = "j",
                    Nombre = "Distribuidora",
                    Estatus = true,
                    TokenSeg = "U5678"
                },
                Servicio = new ServicioEntity
                {
                    Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13"),
                    Nombre = "servicio4",
                    Estatus = Status.activo,
                    Categoria = "hola",
                    TipoPago = "contado",
                    PrestadorEntityId = "username9",
                    Descripcion = "Funciona"
                }
            };
            return pago;
        }

        public static FormatoServicioEntity BuildFormato()
        {
            var formato = new FormatoServicioEntity
            {
                Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a70"),
                ServicioEntityId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13"),
                FormatoConEntityId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a20"),
                Requerido = true,
                Logitud = 10,
                FormatoCon = new FormatoConEntity
                {
                    Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a23"),
                    NombreCampo = "Descripcion",
                    TipoDato = "string"
                }
            };
            return formato;
        }
    }
}
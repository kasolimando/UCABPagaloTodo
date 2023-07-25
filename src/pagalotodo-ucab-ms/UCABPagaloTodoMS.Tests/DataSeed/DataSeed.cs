using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;
using static UCABPagaloTodoMS.Core.Entities.ServicioEntity;

namespace UCABPagaloTodoMS.Tests.DataSeed
{
    public static class DataSeed
    {
        private static Mock<DbSet<Administrador>> mockSetAdmin = new();
        private static Mock<DbSet<ConsumidorEntity>> mockSetConsumidores = new();
        private static Mock<DbSet<PrestadorEntity>> mockSetPrestadores = new();
        private static Mock<DbSet<ServicioEntity>> mockSetServicios = new();
        private static Mock<DbSet<FormatoConEntity>> mockSetCamposFormato = new();
        private static Mock<DbSet<FormatoServicioEntity>> mockSetFormatos = new();
        private static Mock<DbSet<PagoEntity>> mockSetPagos = new();
        private static Mock<DbSet<DeudaEntity>> mockSetDeudas = new();

        public static void SetupDbContextData(this Mock<IUCABPagaloTodoDbContext> _mockContext)
        {
            //Administradores

            var requestAdministrador = new List<Administrador>
             {
                 new Administrador
                 {
                     Username = "username1",
                     Clave = "e1075933b26b5e4e50ab3dc3528eb3461214ba15b7a27b51f5dbc086912caf56",
                     Correo = "username1@gmail.com",
                     DocIdentidad = "1234",
                     TipoVj = "v",
                     Direccion = "Los Samanes",
                     Nombre = "Pedro",
                     Estatus = true
                 }, new Administrador
                 {
                     Username = "username2",
                     Clave = "e1075933b26b5e4e50ab3dc3528eb3461214ba15b7a27b51f5dbc086912caf56",
                     Correo = "username2@gmail.com",
                     DocIdentidad = "5678",
                     TipoVj = "j",
                     Nombre = "Distribuidora",
                     Estatus = true,
                     TokenSeg = "U5678"
                 }, new Administrador
                 {
                     Username = "username3",
                     Clave = "username3",
                     Correo = "username3@gmail.com",
                     DocIdentidad = "1245",
                     TipoVj = "e",
                     Nombre = "Ana",
                     Estatus = false,
                     Direccion = "Los Naranjos"
                 }
                 , new Administrador
                 {
                     Username = "usernameE",
                     Clave = Encriptacion.EncriptarClave("usernameE"),
                     Correo = "usernameE@gmail.com",
                     DocIdentidad = "1245",
                     TipoVj = "e",
                     Nombre = "Ana",
                     Estatus = true,
                     Direccion = "Los Naranjos"
                 }
            };

            var requestConsumidores = new List<ConsumidorEntity>
             {
                 new ConsumidorEntity
                 {
                     Username = "username4",
                     Clave = "c9366c1bd693018b9ac3c43f1404d3063e7c62b73f93a3d3b17ec71411d5f10f",
                     Correo = "username4@gmail.com",
                     DocIdentidad = "1234",
                     TipoVj = "v",
                     Direccion = "Los Samanes",
                     Nombre = "Pedro",
                     Estatus = true
                 }, new ConsumidorEntity
                 {
                     Username = "username5",
                     Clave = "username5",
                     Correo = "username5@gmail.com",
                     DocIdentidad = "5678",
                     TipoVj = "j",
                     Nombre = "Distribuidora",
                     Estatus = true,
                     TokenSeg = "U5678"
                 }, new ConsumidorEntity
                 {
                     Username = "username6",
                     Clave = "username6",
                     Correo = "username6@gmail.com",
                     DocIdentidad = "1245",
                     TipoVj = "e",
                     Nombre = "Ana",
                     Estatus = true,
                     Direccion = "Los Naranjos"
                 }
                 , new ConsumidorEntity
                 {
                     Username = "usernameC",
                     Clave = "usernameC",
                     Correo = "usernameC@gmail.com",
                     DocIdentidad = "1245",
                     TipoVj = "e",
                     Nombre = "Ana",
                     Estatus = false,
                     Direccion = "Los Naranjos"
                 }
             };

            var requestPrestadores = new List<PrestadorEntity>
             {
                 new PrestadorEntity
                 {
                     Username = "username7",
                     Clave = "username7",
                     Correo = "username7@gmail.com",
                     DocIdentidad = "1234",
                     TipoVj = "v",
                     Direccion = "Los Samanes",
                     Nombre = "Pedro",
                     Estatus = true
                 }, new PrestadorEntity
                 {
                     Username = "username8",
                     Clave = "username8",
                     Correo = "username8@gmail.com",
                     DocIdentidad = "5678",
                     TipoVj = "j",
                     Nombre = "Distribuidora",
                     Estatus = true,
                     TokenSeg = "U5678"
                 }, new PrestadorEntity
                 {
                     Username = "username9",
                     Clave = "a3c5b8a503c609276b5ca6988599fd8c7a9ce4e700dd467e3b5a4cb72b381669",
                     Correo = "username9@gmail.com",
                     DocIdentidad = "1245",
                     TipoVj = "e",
                     Nombre = "Ana",
                     Estatus = false,
                     Direccion = "Los Naranjos"
                 }
                 , new PrestadorEntity
                 {
                     Username = "username10",
                     Clave = "username10",
                     Correo = "username10@gmail.com",
                     DocIdentidad = "1245",
                     TipoVj = "e",
                     Nombre = "Ana",
                     Estatus = true,
                     Direccion = "Los Naranjos"
                 }
             };

            var requestServicios = new List<ServicioEntity>
             {
                 new ServicioEntity
                 {
                     Id = Guid.NewGuid(),
                     Nombre = "servicio1",
                     Estatus = Status.activo,
                     Categoria = "hola",
                     TipoPago = "confirmacion",
                     PrestadorEntityId = "username8"
                 },
                new ServicioEntity
                 {
                     Id = Guid.NewGuid(),
                     Nombre = "servicio2",
                     Estatus = Status.activo,
                     Categoria = "hola",
                     TipoPago = "confirmacion",
                     PrestadorEntityId = "username8"

                 },
                new ServicioEntity
                 {
                     Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13"),
                     Nombre = "servicio3",
                     Estatus = Status.activo,
                     Categoria = "hola",
                     TipoPago = "confirmacion",
                     PrestadorEntityId = "username9",
                     FormatoConEntityId =  Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a70"),
                     Formato = new FormatoConEntity
                                 {
                                     Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a20"),
                                     NombreCampo = "Nombre",
                                     TipoDato = "string"
                                 },
                     Deuda = new()
                     {
                         new DeudaEntity
                            {
                                 Username = "username4",
                                 Monto = 90,
                                 Estatus = DeudaEntity.Status.Activo,
                                 servicioId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13")
                            }
                     }
                 },
                new ServicioEntity
                 {
                     Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                     Nombre = "servicio4",
                     Estatus = Status.activo,
                     Categoria = "hola",
                     TipoPago = "contado",
                     PrestadorEntityId = "username9"
                 }
             };

            var requestCamposFormatos = new List<FormatoConEntity>
             {
                 new FormatoConEntity
                 {
                     Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a20"),
                     NombreCampo = "Nombre",
                     TipoDato = "string"
                 },
                new FormatoConEntity
                 {
                    Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a21"),
                    NombreCampo = "Fecha",
                    TipoDato = "Date"

                 },
                new FormatoConEntity
                 {
                    Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a22"),
                    NombreCampo = "Monto",
                    TipoDato = "Double"
                 },
                new FormatoConEntity
                 {
                    Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a23"),
                    NombreCampo = "Descripcion",
                    TipoDato = "string"
                 }
             };


            var requestPagos = new List<PagoEntity> {
                new PagoEntity
                {
                     Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a80"),
                     Monto = 20.5,
                     Fecha = DateTime.Now,
                     Aprobado = false,
                     Cierre = true,
                     ServicioEntityId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13"),
                     ConsumidorEntityId = "username5",
                     FechaCierre = DateTime.Now,
                     Consumidor =  new ConsumidorEntity
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
                                     Nombre = "servicio3",
                                     Estatus = Status.activo,
                                     Categoria = "hola",
                                     TipoPago = "contado",
                                     PrestadorEntityId = "username9"
                                 }
                },
                new PagoEntity
                {
                     Id = Guid.NewGuid(),
                     Monto = 30.5,
                     Fecha = DateTime.Now,
                     Aprobado = false,
                     Cierre = false,
                     ServicioEntityId =Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13"),
                     ConsumidorEntityId = "username4",
                     FechaCierre = DateTime.Now,
                     Consumidor = new ConsumidorEntity
                                 {
                                     Username = "username4",
                                     Clave = "username4",
                                     Correo = "username4@gmail.com",
                                     DocIdentidad = "1234",
                                     TipoVj = "v",
                                     Direccion = "Los Samanes",
                                     Nombre = "Pedro",
                                     Estatus = true
                                 },
                     Servicio = new ServicioEntity
                                 {
                                     Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13"),
                                     Nombre = "servicio3",
                                     Estatus = Status.activo,
                                     Categoria = "hola",
                                     TipoPago = "confirmacion",
                                     PrestadorEntityId = "username9",
                                     FormatoConEntityId =  Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a70"),
                                     Formato = new FormatoConEntity
                                                 {
                                                     Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a20"),
                                                     NombreCampo = "Nombre",
                                                     TipoDato = "string"
                                                 },
                                                     Deuda = new()
                                                     {
                                                         new DeudaEntity
                                                            {
                                                                 Username = "username4",
                                                                 Monto = 90,
                                                                 Estatus = DeudaEntity.Status.Activo,
                                                                 servicioId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13")
                                                            }
                                                     }
                                    }              
                },
                new PagoEntity
                {
                     Id = Guid.NewGuid(),
                     Monto = 70,
                     Fecha = DateTime.Now,
                     Aprobado = true,
                     Cierre = true,
                     ServicioEntityId =Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                     ConsumidorEntityId = "username6",
                     FechaCierre = DateTime.Now
                }
            };

            var requestDeudas = new List<DeudaEntity> {
                new DeudaEntity
                {
                     Username = "username4",
                     Monto = 90,
                     Estatus = DeudaEntity.Status.Activo,
                     servicioId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13"),
                     Servicio = new ServicioEntity
                                 {
                                     Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13"),
                                     Nombre = "servicio3",
                                     Estatus = Status.activo,
                                     Categoria = "hola",
                                     TipoPago = "confirmacion",
                                     PrestadorEntityId = "username9",
                                     FormatoConEntityId =  Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a70"),
                                     Formato = new FormatoConEntity
                                                 {
                                                     Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a20"),
                                                     NombreCampo = "Nombre",
                                                     TipoDato = "string"
                                                 },
                                                     Deuda = new()
                                                     {
                                                         new DeudaEntity
                                                            {
                                                                 Username = "username4",
                                                                 Monto = 90,
                                                                 Estatus = DeudaEntity.Status.Activo,
                                                                 servicioId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13")
                                                            }
                                                     }
                                    }
                },
                new DeudaEntity
                {
                     Username = "username5",
                     Monto = 70,
                     Estatus = DeudaEntity.Status.Inactivo,
                     servicioId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13"),
                      Servicio = new ServicioEntity
                                 {
                                     Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13"),
                                     Nombre = "servicio3",
                                     Estatus = Status.activo,
                                     Categoria = "hola",
                                     TipoPago = "confirmacion",
                                     PrestadorEntityId = "username9",
                                     FormatoConEntityId =  Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a70"),
                                     Formato = new FormatoConEntity
                                                 {
                                                     Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a20"),
                                                     NombreCampo = "Nombre",
                                                     TipoDato = "string"
                                                 },
                                                     Deuda = new()
                                                     {
                                                         new DeudaEntity
                                                            {
                                                                 Username = "username4",
                                                                 Monto = 90,
                                                                 Estatus = DeudaEntity.Status.Activo,
                                                                 servicioId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13")
                                                            }
                                                     }
                                    }
                }
            };

            var requestFormatos = new List<FormatoServicioEntity>
             {
                 new FormatoServicioEntity
                 {
                     Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a70"),
                     ServicioEntityId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13"),
                     FormatoConEntityId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a20"),
                     Requerido = true,
                     Logitud = 10,
                     FormatoCon =  new FormatoConEntity
                                     {
                                         Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a20"),
                                         NombreCampo = "Nombre",
                                         TipoDato = "string"
                                     }
                 },
                new FormatoServicioEntity
                 {
                     Id = Guid.NewGuid(),
                     ServicioEntityId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a13"),
                     FormatoConEntityId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a21"),
                     Requerido = true,
                     Logitud = 7,
                     FormatoCon = new FormatoConEntity
                         {
                            Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a21"),
                            NombreCampo = "Fecha",
                            TipoDato = "Date"

                         }

                 },
                new FormatoServicioEntity
                 {
                     Id = Guid.NewGuid(),
                     ServicioEntityId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                     FormatoConEntityId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a20"),
                     Requerido = true,
                     Logitud = 12,
                     FormatoCon =  new FormatoConEntity
                                     {
                                         Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a20"),
                                         NombreCampo = "Nombre",
                                         TipoDato = "string"
                                     }
                 }
             };



            // Configura el objeto _dbContextMock para simular el DbSet<ConsumidorEntity>
            mockSetAdmin.As<IQueryable<Administrador>>().Setup(c => c.Provider).Returns(requestAdministrador.AsQueryable().Provider);
            mockSetAdmin.As<IQueryable<Administrador>>().Setup(c => c.Expression).Returns(requestAdministrador.AsQueryable().Expression);
            mockSetAdmin.As<IQueryable<Administrador>>().Setup(c => c.ElementType).Returns(requestAdministrador.AsQueryable().ElementType);
            mockSetAdmin.As<IQueryable<Administrador>>().Setup(c => c.GetEnumerator()).Returns(requestAdministrador.AsQueryable().GetEnumerator());

            _mockContext.Setup(t => t.Administrador).Returns(mockSetAdmin.Object);
            _mockContext.Setup(t => t.DbContext.SaveChanges()).Returns(1);
            _mockContext.Setup(c => c.Administrador).Returns(requestAdministrador.AsQueryable().BuildMockDbSet().Object);

            var mockAdminDbSet = requestAdministrador.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Administrador).Returns(mockAdminDbSet.Object);

            // Configura el objeto _dbContextMock para simular el DbSet<ConsumidorEntity>
            mockSetConsumidores.As<IQueryable<ConsumidorEntity>>().Setup(m => m.Provider).Returns(requestConsumidores.AsQueryable().Provider);
            mockSetConsumidores.As<IQueryable<ConsumidorEntity>>().Setup(m => m.Expression).Returns(requestConsumidores.AsQueryable().Expression);
            mockSetConsumidores.As<IQueryable<ConsumidorEntity>>().Setup(m => m.ElementType).Returns(requestConsumidores.AsQueryable().ElementType);
            mockSetConsumidores.As<IQueryable<ConsumidorEntity>>().Setup(m => m.GetEnumerator()).Returns(requestConsumidores.GetEnumerator());

            _mockContext.Setup(t => t.Consumidor).Returns(mockSetConsumidores.Object);
            _mockContext.Setup(t => t.DbContext.SaveChanges()).Returns(1);
            _mockContext.Setup(c => c.Consumidor).Returns(requestConsumidores.AsQueryable().BuildMockDbSet().Object);

            // Configura el objeto _dbContextMock para simular el DbContext
            var mockConsumidorDbSet = requestConsumidores.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Consumidor).Returns(mockConsumidorDbSet.Object);

            // Configura el objeto _dbContextMock para simular el DbSet<PrestadorEntity>
            mockSetPrestadores.As<IQueryable<PrestadorEntity>>().Setup(c => c.Provider).Returns(requestPrestadores.AsQueryable().Provider);
            mockSetPrestadores.As<IQueryable<PrestadorEntity>>().Setup(c => c.Expression).Returns(requestPrestadores.AsQueryable().Expression);
            mockSetPrestadores.As<IQueryable<PrestadorEntity>>().Setup(c => c.ElementType).Returns(requestPrestadores.AsQueryable().ElementType);
            mockSetPrestadores.As<IQueryable<PrestadorEntity>>().Setup(c => c.GetEnumerator()).Returns(requestPrestadores.AsQueryable().GetEnumerator());

            _mockContext.Setup(t => t.Prestador).Returns(mockSetPrestadores.Object);
            _mockContext.Setup(t => t.DbContext.SaveChanges()).Returns(1);
            _mockContext.Setup(c => c.Prestador).Returns(requestPrestadores.AsQueryable().BuildMockDbSet().Object);

            // Configura el objeto _dbContextMock para simular el DbContext
            var mockPrestadorDbSet = requestPrestadores.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Prestador).Returns(mockPrestadorDbSet.Object);

            // Configura el objeto _dbContextMock para simular el DbSet<ServicioEntity>
            mockSetServicios.As<IQueryable<ServicioEntity>>().Setup(c => c.Provider).Returns(requestServicios.AsQueryable().Provider);
            mockSetServicios.As<IQueryable<ServicioEntity>>().Setup(c => c.Expression).Returns(requestServicios.AsQueryable().Expression);
            mockSetServicios.As<IQueryable<ServicioEntity>>().Setup(c => c.ElementType).Returns(requestServicios.AsQueryable().ElementType);
            mockSetServicios.As<IQueryable<ServicioEntity>>().Setup(c => c.GetEnumerator()).Returns(requestServicios.AsQueryable().GetEnumerator());
            
            _mockContext.Setup(t => t.Servicio).Returns(mockSetServicios.Object);
            _mockContext.Setup(t => t.DbContext.SaveChanges()).Returns(1);
            _mockContext.Setup(c => c.Servicio).Returns(requestServicios.AsQueryable().BuildMockDbSet().Object);

            // Configura el objeto _dbContextMock para simular el DbContext
            var mockServicioDbSet = requestServicios.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Servicio).Returns(mockServicioDbSet.Object);

            // Configura el objeto _dbContextMock para simular el DbSet<ServicioEntity>
            mockSetFormatos.As<IQueryable<FormatoConEntity>>().Setup(c => c.Provider).Returns(requestCamposFormatos.AsQueryable().Provider);
            mockSetFormatos.As<IQueryable<FormatoConEntity>>().Setup(c => c.Expression).Returns(requestCamposFormatos.AsQueryable().Expression);
            mockSetFormatos.As<IQueryable<FormatoConEntity>>().Setup(c => c.ElementType).Returns(requestCamposFormatos.AsQueryable().ElementType);
            mockSetFormatos.As<IQueryable<FormatoConEntity>>().Setup(c => c.GetEnumerator()).Returns(requestCamposFormatos.AsQueryable().GetEnumerator());

            _mockContext.Setup(t => t.Formato).Returns(mockSetCamposFormato.Object);
            _mockContext.Setup(t => t.DbContext.SaveChanges()).Returns(1);
            _mockContext.Setup(c => c.Formato).Returns(requestCamposFormatos.AsQueryable().BuildMockDbSet().Object);

            // Configura el objeto _dbContextMock para simular el DbContext
            var mockCamposFormatoDbSet = requestCamposFormatos.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Formato).Returns(mockCamposFormatoDbSet.Object);


            // Configura el objeto _dbContextMock para simular el DbSet<PagoEntity>
            mockSetPagos.As<IQueryable<PagoEntity>>().Setup(c => c.Provider).Returns(requestPagos.AsQueryable().Provider);
            mockSetPagos.As<IQueryable<PagoEntity>>().Setup(c => c.Expression).Returns(requestPagos.AsQueryable().Expression);
            mockSetPagos.As<IQueryable<PagoEntity>>().Setup(c => c.ElementType).Returns(requestPagos.AsQueryable().ElementType);
            mockSetPagos.As<IQueryable<PagoEntity>>().Setup(c => c.GetEnumerator()).Returns(requestPagos.AsQueryable().GetEnumerator());

            _mockContext.Setup(t => t.Pago).Returns(mockSetPagos.Object);
            _mockContext.Setup(t => t.DbContext.SaveChanges()).Returns(1);
            _mockContext.Setup(c => c.Pago).Returns(requestPagos.AsQueryable().BuildMockDbSet().Object);

            // Configura el objeto _dbContextMock para simular el DbContext
            var mockPagoDbSet = requestPagos.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Pago).Returns(mockPagoDbSet.Object);

            // Configura el objeto _dbContextMock para simular el DbSet<DeudaEntity>
            mockSetDeudas.As<IQueryable<DeudaEntity>>().Setup(c => c.Provider).Returns(requestDeudas.AsQueryable().Provider);
            mockSetDeudas.As<IQueryable<DeudaEntity>>().Setup(c => c.Expression).Returns(requestDeudas.AsQueryable().Expression);
            mockSetDeudas.As<IQueryable<DeudaEntity>>().Setup(c => c.ElementType).Returns(requestDeudas.AsQueryable().ElementType);
            mockSetDeudas.As<IQueryable<DeudaEntity>>().Setup(c => c.GetEnumerator()).Returns(requestDeudas.AsQueryable().GetEnumerator());

            _mockContext.Setup(t => t.Deuda).Returns(mockSetDeudas.Object);
            _mockContext.Setup(t => t.DbContext.SaveChanges()).Returns(1);
            _mockContext.Setup(c => c.Deuda).Returns(requestDeudas.AsQueryable().BuildMockDbSet().Object);

            // Configura el objeto _dbContextMock para simular el DbContext
            var mockDeudaDbSet = requestDeudas.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Deuda).Returns(mockDeudaDbSet.Object);

            // Configura el objeto _dbContextMock para simular el DbSet<FormatoServicioEntity>
            mockSetFormatos.As<IQueryable<FormatoServicioEntity>>().Setup(c => c.Provider).Returns(requestFormatos.AsQueryable().Provider);
            mockSetFormatos.As<IQueryable<FormatoServicioEntity>>().Setup(c => c.Expression).Returns(requestFormatos.AsQueryable().Expression);
            mockSetFormatos.As<IQueryable<FormatoServicioEntity>>().Setup(c => c.ElementType).Returns(requestFormatos.AsQueryable().ElementType);
            mockSetFormatos.As<IQueryable<FormatoServicioEntity>>().Setup(c => c.GetEnumerator()).Returns(requestFormatos.AsQueryable().GetEnumerator());

            _mockContext.Setup(t => t.FormatoServicio).Returns(mockSetFormatos.Object);
            _mockContext.Setup(t => t.DbContext.SaveChanges()).Returns(1);
            _mockContext.Setup(c => c.FormatoServicio).Returns(requestFormatos.AsQueryable().BuildMockDbSet().Object);


            // Configura el objeto _dbContextMock para simular el DbContext
            var mockFormatoDbSet = requestFormatos.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.FormatoServicio).Returns(mockFormatoDbSet.Object);
        }
    }
}

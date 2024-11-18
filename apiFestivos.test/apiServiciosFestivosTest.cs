using apiFestivos.Aplicacion.Servicios;
using apiFestivos.Core.Interfaces.Repositorios;
using apiFestivos.Core.Interfaces.Servicios;
using apiFestivos.Dominio.Entidades;
using Moq;

namespace apiFestivos.test
{
    public class EsFestivoRepositorioTest
    {
        private readonly Mock<IFestivoRepositorio> festivoRepositorioMock;
        private readonly IFestivoServicio festivoServicio;

        public EsFestivoRepositorioTest()
        {
            festivoRepositorioMock = new Mock<IFestivoRepositorio>();
            festivoServicio = new FestivoServicio(festivoRepositorioMock.Object);
        }
        [Fact]
        public async Task verificaCoincideFechaConFestivo()
        {
            // Arrange
            var festivos = new List<Festivo>
            {
                new Festivo { Id = 1, Nombre = "Año Nuevo 1", Dia = 1, Mes = 1, IdTipo = 1 },
                new Festivo { Id = 2, Nombre = "Santos Reyes", Dia = 6, Mes = 1, IdTipo = 2 },
                new Festivo { Id = 3, Nombre = "Ascensión del Señor", DiasPascua = 40, IdTipo = 4 },
                new Festivo { Id = 4, Nombre = "Año Nuevo 4", Dia = 4, Mes = 1, IdTipo = 2 },
                new Festivo { Id = 5, Nombre = "Año Nuevo 5", Dia = 5, Mes = 1, IdTipo = 3 }

            };
            festivoRepositorioMock.Setup(repo => repo.ObtenerTodos()).ReturnsAsync(festivos);

            var fechaFestiva = new DateTime(2024, 1, 1);

            // Act
            var esFestivo = await festivoServicio.EsFestivo(fechaFestiva);

            // Assert
            Assert.True(esFestivo, "La fecha debería ser festiva, pero no lo fue.");
        }

        [Fact]
        public async Task FechaNoFestivaDeberiaDevolverFalse()
        {
            // Arrange
            var festivos = new List<Festivo>
            {
                new Festivo { Id = 1, Nombre = "Año Nuevo", Dia = 1, Mes = 1, IdTipo = 1 },
                new Festivo { Id = 2, Nombre = "Día de la Independencia", Dia = 4, Mes = 7, IdTipo = 1 }
            };

            festivoRepositorioMock.Setup(repo => repo.ObtenerTodos()).ReturnsAsync(festivos);

            var fechaNoFestiva = new DateTime(2024, 2, 15); // Una fecha que no está en la lista.

            // Act
            var esFestivo = await festivoServicio.EsFestivo(fechaNoFestiva);

            // Assert
            Assert.False(esFestivo, "La fecha no debería ser festiva, pero el método devolvió true.");
        }
    }
}

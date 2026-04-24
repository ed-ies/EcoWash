using EcoWash.ViewModels;

namespace TestProject1
{
    [TestClass]
    public class TiendasCrudViewModelTests
    {
        // Testeamos el comando de NuevaTiendaCommand
        // Creamos una nueva tienda y comprobamos que los datos existan en memoria
        [TestMethod]
        public void Test_NuevaTiendaCommand()
        {
            var vm = new TiendasCrudViewModel();

            vm.NuevaTiendaCommand.Execute(null);

            Assert.AreEqual(0, vm.Id);
            Assert.AreEqual(1, vm.Lavadoras.Count);
            Assert.IsFalse(vm.Lavadoras[0].IsOccupied);
        }

        // Testeamos el comando de NuevaTiendaCommand
        // Creamos una nueva tienda ficticia y le añadimos una lavadora, si el conteo llega a 2 , se ha hecho correctamente
        [TestMethod]
        public void Test_NuevaLavadoraCommand()
        {
            var vm = new TiendasCrudViewModel();
            vm.NuevaTiendaCommand.Execute(null);

            vm.NuevaLavadoraCommand.Execute(null);

            Assert.AreEqual(2, vm.Lavadoras.Count);
        }

        // Testeamos el comando de BorrarLavadoraCommand
        // Creamos una tienda con su lavadora y la intentamos eliminar, como siempre ha de haber una, la lavadora debe de seguir ahi
        [TestMethod]
        public void Test_BorrarLavadoraCommand()
        {
            var vm = new TiendasCrudViewModel();
            vm.NuevaTiendaCommand.Execute(null);
            vm.NuevaLavadoraCommand.Execute(null);

            // Eliminamos la primera lavadora
            var lavadoraAEliminar = vm.Lavadoras.First();
            vm.BorrarLavadoraCommand.Execute(lavadoraAEliminar);

            // Intentamos eliminar la segunda lavadora
            lavadoraAEliminar = vm.Lavadoras.First();
            vm.BorrarLavadoraCommand.Execute(lavadoraAEliminar);


            Assert.AreEqual(1, vm.Lavadoras.Count);
        }
    }
}


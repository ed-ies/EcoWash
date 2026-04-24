using EcoWash.Models;
using EcoWash.Services;
using EcoWash.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EcoWash.ViewModels
{
    public class PanelTiendasViewModel : BaseViewModel
    {
        #region atributos
        // Coleccion para almacenar las tiendas de la BBDD
        private List<Tienda> _listaTiendas;
        private ObservableCollection<Tienda> _listaTiendasVisibles;

        #endregion

        #region Propiedades

        // Tiendas disponibles
        public ObservableCollection<Tienda> ListaTiendasVisibles
        {
            get => _listaTiendasVisibles;
            set => SetProperty(ref _listaTiendasVisibles, value);
        }

        #endregion

        #region Constructores

        // Delcaramos los comandos
        public ICommand CargarCommand { get; }

        public PanelTiendasViewModel()
        {
            // Inicializamos los atributos
            _listaTiendas = new();
            _listaTiendasVisibles = new();

            // Inicializamos la lista de tiendas visibles
            ListaTiendasVisibles = new ObservableCollection<Tienda>();

            // Inicializamos los comandos
            CargarCommand = new RelayCommand(PerformCargarTiendas);
        }

        #endregion

        #region Metodos
        private async void PerformCargarTiendas(object? parameter = null)
        {
            // Abrimos una conexion con la BBDD
            var service = new ServiceTienda();

            // Cargamos las tiendas desde la BBDD
            _listaTiendas = await service.Listar();

            // Cargamos las tiendas en la ObservableCollection
            ListaTiendasVisibles.Clear();
            foreach (var s in _listaTiendas)
            {
                ListaTiendasVisibles.Add(s);
            }
        }

        #endregion
    }
}

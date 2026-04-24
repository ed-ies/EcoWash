using EcoWash.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EcoWash.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Atributos

        // Propiedad que guarda el ViewModel de la vista actual
        private object _currentView;

        #endregion

        #region Propiedades

        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value); // Lo establecemos usando el helper para solo notificar si hay cambios
        }

        #endregion

        #region Constructores
        // Comandos para navegar
        public ICommand ShowMainCommand { get; }
        public ICommand ShowTiendasCommand { get; }
        public ICommand ShowTiendasCrudCommand { get; }

        public MainWindowViewModel()
        {
            // Inicializamos comandos
            ShowMainCommand = new RelayCommand(ExecuteShowMain);
            ShowTiendasCommand = new RelayCommand(ExecuteShowTiendas);
            ShowTiendasCrudCommand = new RelayCommand(ExecuteShowTiendasCrud);

            // Al arrancar, mostramos las estaciones por defecto
            ExecuteShowMain();
        }

        #endregion

        #region Metodos

        private void ExecuteShowMain(object? parameter = null)
        {
            // Aquí instanciamos el VM de la vista hija.
            // En un caso real, usaríamos Inyección de Dependencias.
            CurrentView = new PrincipalViewModel();
        }
        private void ExecuteShowTiendas(object? parameter = null)
        {
            // Aquí instanciamos el VM de la vista hija.
            // En un caso real, usaríamos Inyección de Dependencias.
            CurrentView = new PanelTiendasViewModel();
        }
        private void ExecuteShowTiendasCrud(object? parameter = null)
        {
            // Aquí instanciamos el VM de la vista hija.
            // En un caso real, usaríamos Inyección de Dependencias.
            CurrentView = new TiendasCrudViewModel();
        }

        #endregion
    }
}

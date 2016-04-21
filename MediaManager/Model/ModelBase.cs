// Developed by: Gabriel Duarte
// 
// Created at: 12/12/2015 03:00

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using MediaManager.Localizacao;

namespace MediaManager.Model
{
    public class ModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public virtual bool ValidarCampo(object value = null, [CallerMemberName] string propertyName = "")
        {
            if (value == null || (value is string && string.IsNullOrWhiteSpace(value as string)))
            {
                AddError(Mensagens.Campo_de_preenchimento_obrigatório, propertyName);
                return false;
            }

            RemoveError(propertyName);
            return true;
        }

        public void Clone(object objOrigem)
        {
            PropertyInfo[] variaveisObjOrigem = objOrigem.GetType().GetProperties();
            PropertyInfo[] variaveisObjAtual = GetType().GetProperties();

            foreach (PropertyInfo item in variaveisObjOrigem)
            {
                PropertyInfo variavelIgual =
                    variaveisObjAtual.FirstOrDefault(x => x.Name == item.Name && x.PropertyType == item.PropertyType);

                if (variavelIgual != null && variavelIgual.CanWrite)
                {
                    variavelIgual.SetValue(this, item.GetValue(objOrigem, null));
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged Members

        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private readonly Dictionary<string, List<string>> _erros = new Dictionary<string, List<string>>();

        protected void RaiseErrorsChanged([CallerMemberName] string propertyName = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public bool HasErrors => _erros.Count > 0;

        public virtual bool IsValid => !HasErrors;

        public void AddError(string erro, [CallerMemberName] string propertyName = "")
        {
            _erros[propertyName] = new List<string> {erro};
            RaiseErrorsChanged(propertyName);
        }

        public void RemoveError([CallerMemberName] string propertyName = "")
        {
            if (_erros.ContainsKey(propertyName))
            {
                _erros.Remove(propertyName);
            }

            RaiseErrorsChanged(propertyName);
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName != null && _erros.ContainsKey(propertyName))
            {
                return _erros[propertyName];
            }

            return null;
        }

        #endregion INotifyDataErrorInfo
    }
}

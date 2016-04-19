using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MediaManager.Model
{
    public class ModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public virtual bool ValidarCampo(object value = null, [CallerMemberName] string propertyName = "")
        {
            if (value == null || (value is string && string.IsNullOrWhiteSpace(value as string)))
            {
                AddError("Campo de preenchimento obrigatório.", propertyName);
                return false;
            }
            else
            {
                RemoveError(propertyName);
                return true;
            }
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

            return;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members

        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private readonly Dictionary<string, List<string>> _erros = new Dictionary<string, List<string>>();

        protected void RaiseErrorsChanged([CallerMemberName] string propertyName = "")
        {
            EventHandler<DataErrorsChangedEventArgs> handler = ErrorsChanged;

            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public bool HasErrors
        {
            get { return _erros.Count > 0; }
        }

        public virtual bool IsValid
        {
            get { return !HasErrors; }
        }

        public void AddError(string erro, [CallerMemberName] string propertyName = "")
        {
            _erros[propertyName] = new List<string>() {erro};
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
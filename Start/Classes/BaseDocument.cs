using Dapper.Contrib.Extensions;
using Start.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Start.Common.Classes
{
    public class BaseDocument<T> : IBaseDocument<T>
        where T : class
    {
        public string JSONDocument { get; set; }

        [Computed]
        [JsonIgnore]
        public T Document {
            get
            {
                if (_document == null)
                {
                    if (string.IsNullOrEmpty(JSONDocument))
                    {
                        JSONDocument = "{}";
                    }

                    _document = this.DeserializeDocument();
                }

                return _document;
            }
            set
            {
                _document = value;
                ApplyChanges();
            }
        }

        [Computed]
        [JsonIgnore]
        private T _document { get; set; }

        public void ApplyChanges()
        {
            JSONDocument = JsonSerializer.Serialize(_document);
        }

        protected virtual T DeserializeDocument()
        {
            return JsonSerializer.Deserialize<T>(this.JSONDocument);
        }
    }
}

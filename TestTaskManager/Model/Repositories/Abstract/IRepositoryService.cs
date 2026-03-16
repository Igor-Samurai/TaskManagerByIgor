using System;
using System.Collections.Generic;
using System.Text;

namespace TestTaskManager.Model.Repositories.Abstract
{
    public interface IRepositoryService<T> where T : class
    {
        IEnumerable<T> GetAll();
        void Create(T item); // создание объекта
        void Update(T item); // обновление объекта
        void Delete(T item); // удаление объекта по id

    }
}

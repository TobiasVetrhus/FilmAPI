using FilmAPI.Data.Exceptions;

namespace FilmAPI.Services
{
    public interface ICrudService<T, ID>
    {
        /// <summary>
        /// Gets all the instances of <typeparamref name="T"/> in the database.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets a specific <typeparamref name="T"/> by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <returns></returns>
        Task<T> GetByIdAsync(ID id);

        /// <summary>
        /// Adds a <typeparamref name="T"/> to the database.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task<T> AddAsync(T obj);

        /// <summary>
        /// Updates a <typeparamref name="T"/> in the database. 
        /// Any null fields, will be updated with null values. 
        /// This also does not add/update related entities in the database.
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <returns></returns>
        Task<T> UpdateAsync(T obj);

        /// <summary>
        /// Deletes a <typeparamref name="T"/> by their ID.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <returns></returns>
        Task DeleteAsync(ID id);

    }

}

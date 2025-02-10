/*Fonction afin de définir un temps pour éviter les call intempestif en cas de tappe rapide sur le clavier  */
export function debounce(func, delay) {
    let debounceTimeout;
    return (...args) => {
        clearTimeout(debounceTimeout);
        debounceTimeout = setTimeout(() => func(...args), delay);
    };
}
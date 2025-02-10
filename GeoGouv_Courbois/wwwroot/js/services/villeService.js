/*Appel à la route du controller Ville qui me renverra le resultat des villes en fonction des paramètres de trie et / ou de recherche */
export async function getVilles(page, pageSize, sortBy, ascending, search) {
    try {
        const response = await fetch(`/Ville/GetPaginatedVilles?page=${page}&pageSize=${pageSize}&sortBy=${sortBy}&ascending=${ascending}&search=${encodeURIComponent(search)}`);

        if (!response.ok) throw new Error("Error while trying to get city datas");

        return await response.json();
    } catch (error) {
        console.error("Error on controller", error);
        return [];
    }
}

/*Permet d'appeler sur la route du controller Ville la methode qui inserera les données*/
export function fetchDetailsVille(code) {

    fetch(`/Ville/GetDetails?code=${code}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Erreur while trying to get detais");
            }
            return response.json();
        })
        .then(data => {
            alert("Les données sont désormais présentes en base, retrouvez les dans l'onglet Mes Insert");
        })
        .catch(error => {
            console.error("Error:", error);
            alert("Impossible to get details.");
        });
}


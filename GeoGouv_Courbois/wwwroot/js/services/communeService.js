export async function getCommunes(page, pageSize, sortBy, ascending, search) {
    try {
        const response = await fetch(`/Commune/GetPaginatedCommunes?page=${page}&pageSize=${pageSize}&sortBy=${sortBy}&ascending=${ascending}&search=${encodeURIComponent(search)}`);

        if (!response.ok) throw new Error("Erreur lors de la récupération des communes");

        return await response.json();
    } catch (error) {
        console.error("Erreur sur le contrôleur", error);
        return [];
    }
}

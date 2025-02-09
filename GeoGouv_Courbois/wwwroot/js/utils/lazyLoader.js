import { getVilleToDisplay, getIsLoading } from "../pages/villePage.js";

export function setupLazyLoaderOnScroll() {
    window.addEventListener('scroll', () => {
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight - 100 && !getIsLoading()) {

            var ascending = sessionStorage.getItem("ascending", ascending);
            var sortBy = sessionStorage.getItem("sortBy", sortBy);

            getVilleToDisplay(false, sortBy, ascending);
        }
    });
}

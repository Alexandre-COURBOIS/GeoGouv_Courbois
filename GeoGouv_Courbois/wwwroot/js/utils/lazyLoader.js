import { getCommunesToDisplay, getIsLoading } from "../pages/communePage.js";

export function setupLazyLoaderOnScroll() {
    window.addEventListener('scroll', () => {
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight - 100 && !getIsLoading()) {

            var ascending = sessionStorage.getItem("ascending", ascending);
            var sortBy = sessionStorage.getItem("sortBy", sortBy);

            getCommunesToDisplay(false, sortBy, ascending);
        }
    });
}

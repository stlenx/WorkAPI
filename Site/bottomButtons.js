const rightCard = document.getElementById("bottomRightButtonCard");

function ShowBottomRightButtonCard() {
    rightCard.style.display = "unset";
}

function HideBottomRightButtonCard() {
    rightCard.style.display = "none";
}

if(rightCard !== undefined) {
    rightCard.addEventListener("click", (ev => {
        if(ev.target.id === rightCard.id) {
            HideBottomRightButtonCard();
        }
    }));
}



const leftCard = document.getElementById("bottomLeftButtonCard");

function ShowBottomLeftButtonCard() {
    leftCard.style.display = "unset";
}

function HideBottomLeftButtonCard() {
    leftCard.style.display = "none";
}

if(leftCard !== undefined) {
    leftCard.addEventListener("click", (ev => {
        if(ev.target.id === leftCard.id) {
            HideBottomLeftButtonCard();
        }
    }));
}

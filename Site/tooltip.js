let tooltip = document.getElementById("tooltip");

function ShowTooltip() {
    tooltip.style.display = "unset";
}

function HideTooltip() {
    tooltip.style.display = "none";
}

function SetTooltip(text) {
    tooltip.innerText = text;
}

function SetToolTipStyle(backgroundColor, textColor) {
    tooltip.style.backgroundColor = backgroundColor;
    tooltip.style.color = textColor;
}

if(tooltip !== undefined) {
    window.addEventListener("mousemove", (e) => {
        tooltip.style.left = `${e.x}px`;
        tooltip.style.top = `${e.y}px`;
    })
}
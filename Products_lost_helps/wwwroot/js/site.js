let canvas = document.getElementById("imageCanvas");
let ctx = canvas.getContext("2d");

let backgroundImage = new Image();
let overlayImage = new Image();

let isDragging = false;
let dragOffset = { x: 0, y: 0 };
let overlayPosition = { x: 50, y: 50, width: 100, height: 100 };

document.getElementById("backgroundInput").addEventListener("change", (event) => {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
            backgroundImage.src = e.target.result;
            backgroundImage.onload = () => {
                ctx.drawImage(backgroundImage, 0, 0, canvas.width, canvas.height);
                drawOverlay();
            };
        };
        reader.readAsDataURL(file);
    }
});

document.getElementById("overlayInput").addEventListener("change", (event) => {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
            overlayImage.src = e.target.result;
            overlayImage.onload = () => {
                drawOverlay();
            };
        };
        reader.readAsDataURL(file);
    }
});

canvas.addEventListener("mousedown", (event) => {
    const { offsetX, offsetY } = event;
    if (
        offsetX > overlayPosition.x &&
        offsetX < overlayPosition.x + overlayPosition.width &&
        offsetY > overlayPosition.y &&
        offsetY < overlayPosition.y + overlayPosition.height
    ) {
        isDragging = true;
        dragOffset.x = offsetX - overlayPosition.x;
        dragOffset.y = offsetY - overlayPosition.y;
    }
});

canvas.addEventListener("mousemove", (event) => {
    if (isDragging) {
        overlayPosition.x = event.offsetX - dragOffset.x;
        overlayPosition.y = event.offsetY - dragOffset.y;
        redrawCanvas();
    }
});

canvas.addEventListener("mouseup", () => {
    isDragging = false;
});

document.getElementById("saveButton").addEventListener("click", () => {
    const link = document.createElement("a");
    link.download = "composite_image.png";
    link.href = canvas.toDataURL("image/png");
    link.click();
});

function drawOverlay() {
    if (overlayImage.src) {
        ctx.drawImage(
            overlayImage,
            overlayPosition.x,
            overlayPosition.y,
            overlayPosition.width,
            overlayPosition.height
        );
    }
}

function redrawCanvas() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.drawImage(backgroundImage, 0, 0, canvas.width, canvas.height);
    drawOverlay();
}

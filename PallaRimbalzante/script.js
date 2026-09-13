const ball = document.getElementById('ball');
const obstacle = document.getElementById('obstacle');
const btn = document.getElementById('start-stop-btn');
const counterDisplay = document.getElementById('counter');

let isRunning = false;
let animationId = null;
let bounces = 0;

let x = 10;
let y = 10;
let dx = 3;
let dy = 3;

// dimensioni oggetti
const ballSize = 20;
const areaSize = 400;
const obstacleSize = 40;
const obstacleX = 180;
const obstacleY = 180;

// funzione colore casuale
function getRandomColor() {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

// Loop del gioco
function updateGame() {
    if (!isRunning) return;

    // Aggiorna posizione
    x += dx;
    y += dy;

    let hitEdge = false;

    // --- collisione borfi ---
    if (x <= 0) {
        x = 0;
        dx = -dx;
        hitEdge = true;
    } else if (x >= areaSize - ballSize) {
        x = areaSize - ballSize;
        dx = -dx;
        hitEdge = true;
    }

    if (y <= 0) {
        y = 0;
        dy = -dy;
        hitEdge = true;
    } else if (y >= areaSize - ballSize) {
        y = areaSize - ballSize;
        dy = -dy;
        hitEdge = true;
    }

    if (hitEdge) {
        bounces++;
        counterDisplay.textContent = bounces;
    }

    // --- collisione ostacolo al centro ---
    if (x < obstacleX + obstacleSize &&
        x + ballSize > obstacleX &&
        y < obstacleY + obstacleSize &&
        y + ballSize > obstacleY) {
        
        // cambio colore
        ball.style.backgroundColor = getRandomColor();

        // Calcoliamo di quanti pixel la pallina è "entrata" nell'ostacolo dai 4 lati
        let overlapLeft = (x + ballSize) - obstacleX;
        let overlapRight = (obstacleX + obstacleSize) - x;
        let overlapTop = (y + ballSize) - obstacleY;
        let overlapBottom = (obstacleY + obstacleSize) - y;

        // Troviamo la sovrapposizione minore per capire quale lato ha colpito
        let minOverlap = Math.min(overlapLeft, overlapRight, overlapTop, overlapBottom);

        // Respingiamo la pallina fuori dall'ostacolo e invertiamo la velocità
        if (minOverlap === overlapLeft) {
            x = obstacleX - ballSize; // Spingi fuori a sinistra
            dx = -dx;                 // Rimbalza orizzontalmente
        } else if (minOverlap === overlapRight) {
            x = obstacleX + obstacleSize; // Spingi fuori a destra
            dx = -dx;
        } else if (minOverlap === overlapTop) {
            y = obstacleY - ballSize; // Spingi fuori in alto
            dy = -dy;                 // Rimbalza verticalmente
        } else if (minOverlap === overlapBottom) {
            y = obstacleY + obstacleSize; // Spingi fuori in basso
            dy = -dy;
        }
    }

    ball.style.left = x + 'px';
    ball.style.top = y + 'px';

    animationId = requestAnimationFrame(updateGame);
}

//pulsante start/stop
btn.addEventListener('click', function() {
    isRunning = !isRunning;

    if (isRunning) {
        btn.textContent = 'Stop';
        btn.classList.replace('btn-success', 'btn-danger');
        updateGame();
    } else {
        btn.textContent = 'Start';
        btn.classList.replace('btn-danger', 'btn-success');
        cancelAnimationFrame(animationId);
    }
});
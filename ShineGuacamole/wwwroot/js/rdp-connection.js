
let connection = new Map();

export function Connect(id, displayElement, args) {

    if (connection.get(id)) return;

    const tunnel = new Guacamole.WebSocketTunnel("/connect");
    const guac = new Guacamole.Client(tunnel);

    connection.set(id, guac);
    // Add client to display div
    displayElement.appendChild(guac.getDisplay().getElement());

    // Error handler
    guac.onerror = function (error) {
        console.log(error);
        alert(error.message);
    };

    // Mouse
    const mouse = new Guacamole.Mouse(guac.getDisplay().getElement());

    mouse.onmousedown =
        mouse.onmouseup =
        mouse.onmousemove = function (mouseState) {
            guac.sendMouseState(mouseState);
        };

    // Keyboard
    const keyboard = new Guacamole.Keyboard(document);

    keyboard.onkeydown = function (keysym) {
        guac.sendKeyEvent(1, keysym);
    };

    keyboard.onkeyup = function (keysym) {
        guac.sendKeyEvent(0, keysym);
    };

    // Connect
    guac.connect(args);
}

export function Disconnect(id, displayElement) {
    displayElement.innerHTML = '';
    const guac = connection.get(id);
    if (guac) {
        connection.delete(id);
        guac.disconnect();
    }
}
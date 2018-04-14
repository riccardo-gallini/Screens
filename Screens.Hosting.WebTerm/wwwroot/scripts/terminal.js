var Terminal = /** @class */ (function () {
    function Terminal(conn, term) {
        var _this = this;
        this.conn = conn;
        this.conn.on("Beep", function () { return _this.Beep; });
        this.conn.on("Clear", function () { return _this.Clear; });
        this.conn.on("HideCursor", function () { return _this.HideCursor; });
        this.conn.on("SetScreenSize", function (w, h) { return _this.SetScreenSize(w, h); });
        this.conn.on("ShowCursor", function () { return _this.ShowCursor; });
        this.conn.on("SetCursorPosition", function (x, y) { return _this.SetCursorPosition(x, y); });
        this.conn.on("SubmitChanges", function (changes) { return _this.SubmitChanges(changes); });
        this.termControl = term;
        this.termControl.addEventListener("keydown", function (evt) { return _this.KeyDown(evt); });
    }
    Terminal.prototype.initTerm = function () {
        this.termControl.innerHTML = "";
        for (var i = 0; i < this.height; i++) {
            var lineSpan = document.createElement("span");
            lineSpan.id = "L" + i;
            lineSpan.className = "term_line";
            lineSpan.appendChild(this.emptyText(this.width));
            this.termControl.appendChild(lineSpan);
            var cr = document.createTextNode("\n");
            this.termControl.appendChild(cr);
        }
    };
    ;
    Terminal.prototype.emptyText = function (n) {
        var emp = "";
        for (var j = 0; j < n; j++)
            emp += " ";
        var emptyLine = document.createTextNode(emp);
        return emptyLine;
    };
    //expect all spans are consecutive
    Terminal.prototype.drawLine = function (line) {
        var lineSpan = document.getElementById("L" + line.y);
        lineSpan.innerHTML = "";
        for (var _i = 0, _a = line.spans; _i < _a.length; _i++) {
            var span = _a[_i];
            var hs = document.createElement("span");
            hs.style.backgroundColor = get_color(span.backColor);
            hs.style.color = get_color(span.foreColor);
            var t = document.createTextNode(span.text);
            hs.appendChild(t);
            lineSpan.appendChild(hs);
        }
    };
    Terminal.prototype.KeyDown = function (evt) {
        this.conn.send("SendKey", evt.key);
    };
    Terminal.prototype.Beep = function () {
    };
    Terminal.prototype.Clear = function () {
        this.initTerm();
    };
    Terminal.prototype.HideCursor = function () {
    };
    Terminal.prototype.SetScreenSize = function (w, h) {
        this.height = h;
        this.width = w;
        this.initTerm();
    };
    Terminal.prototype.ShowCursor = function () {
    };
    Terminal.prototype.SetCursorPosition = function (x, y) {
    };
    Terminal.prototype.SubmitChanges = function (changes) {
        for (var _i = 0, _a = changes.lines; _i < _a.length; _i++) {
            var changed_line = _a[_i];
            this.drawLine(changed_line);
        }
    };
    return Terminal;
}());
var TerminalChanges = /** @class */ (function () {
    function TerminalChanges() {
    }
    return TerminalChanges;
}());
var ChangedLine = /** @class */ (function () {
    function ChangedLine() {
    }
    return ChangedLine;
}());
var Span = /** @class */ (function () {
    function Span() {
    }
    return Span;
}());
function get_color(inp) {
    switch (inp) {
        case ConsoleColor.Black:
            return '#000000';
        case ConsoleColor.DarkBlue:
            return '#000080';
        case ConsoleColor.DarkGreen:
            return '#008000';
        case ConsoleColor.DarkCyan:
            return '#008080';
        case ConsoleColor.DarkRed:
            return '#800000';
        case ConsoleColor.DarkMagenta:
            return '#800080';
        case ConsoleColor.DarkYellow:
            return '#808000';
        case ConsoleColor.Gray:
            return '#C0C0C0';
        case ConsoleColor.DarkGray:
            return '#808080';
        case ConsoleColor.Blue:
            return '#0000FF';
        case ConsoleColor.Green:
            return '#00FF00';
        case ConsoleColor.Cyan:
            return '#00FFFF';
        case ConsoleColor.Red:
            return '#FF0000';
        case ConsoleColor.Magenta:
            return '#FF00FF';
        case ConsoleColor.Yellow:
            return '#FFFF00';
        case ConsoleColor.White:
            return '#FFFFFF';
        default:
            return '#000000';
    }
}
var ConsoleColor;
(function (ConsoleColor) {
    ConsoleColor[ConsoleColor["Black"] = 0] = "Black";
    ConsoleColor[ConsoleColor["DarkBlue"] = 1] = "DarkBlue";
    ConsoleColor[ConsoleColor["DarkGreen"] = 2] = "DarkGreen";
    ConsoleColor[ConsoleColor["DarkCyan"] = 3] = "DarkCyan";
    ConsoleColor[ConsoleColor["DarkRed"] = 4] = "DarkRed";
    ConsoleColor[ConsoleColor["DarkMagenta"] = 5] = "DarkMagenta";
    ConsoleColor[ConsoleColor["DarkYellow"] = 6] = "DarkYellow";
    ConsoleColor[ConsoleColor["Gray"] = 7] = "Gray";
    ConsoleColor[ConsoleColor["DarkGray"] = 8] = "DarkGray";
    ConsoleColor[ConsoleColor["Blue"] = 9] = "Blue";
    ConsoleColor[ConsoleColor["Green"] = 10] = "Green";
    ConsoleColor[ConsoleColor["Cyan"] = 11] = "Cyan";
    ConsoleColor[ConsoleColor["Red"] = 12] = "Red";
    ConsoleColor[ConsoleColor["Magenta"] = 13] = "Magenta";
    ConsoleColor[ConsoleColor["Yellow"] = 14] = "Yellow";
    ConsoleColor[ConsoleColor["White"] = 15] = "White";
})(ConsoleColor || (ConsoleColor = {}));
function setup() {
    var termControl = document.getElementById("term");
    var connection = new signalR.HubConnection("/hubs/term");
    connection.logging = true;
    var Term = new Terminal(connection, termControl);
    connection.start();
}
setup();
//# sourceMappingURL=terminal.js.map
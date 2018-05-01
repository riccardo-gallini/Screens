class Terminal
{
    conn: any;            //signalR connection that receives updates from server
    termControl: HTMLElement;    //term dom element used for display

    height: number;
    width: number;

    constructor(conn, term: HTMLElement)
    {
        this.conn = conn;
        this.conn.on("Beep", ()=>this.Beep);
        this.conn.on("Clear", ()=>this.Clear);
        this.conn.on("HideCursor", ()=>this.HideCursor);
        this.conn.on("SetScreenSize", (w,h)=>this.SetScreenSize(w,h));
        this.conn.on("ShowCursor", ()=>this.ShowCursor);
        this.conn.on("SetCursorPosition", (x,y)=>this.SetCursorPosition(x,y));
        this.conn.on("SubmitChanges", (changes)=>this.SubmitChanges(changes));

        this.termControl = term;
        this.termControl.addEventListener("keydown", (evt)=>this.KeyDown(evt));
    }



    initTerm()
    {
        this.termControl.innerHTML = "";
        for (let i = 0; i < this.height; i++)
        {
            let lineSpan = document.createElement("span");

            lineSpan.id = "L" + i;
            lineSpan.className = "term_line";
            lineSpan.appendChild(this.emptyText(this.width));

            this.termControl.appendChild(lineSpan);

            let cr = document.createTextNode("\n");        
            this.termControl.appendChild(cr);
        }
    };

    emptyText(n: number): Node
    {
        let emp = "";
        for (let j = 0; j < n; j++)
            emp += " ";
        
        let emptyLine = document.createTextNode(emp);

        return emptyLine;
    }
       
    //expect all spans are consecutive
    drawLine(line: ChangedLine)
    {
        let lineSpan = document.getElementById("L" + line.y);
        lineSpan.innerHTML = "";

        for (let span of line.spans)
        {
            let hs = document.createElement("span");
            hs.style.backgroundColor = get_color(span.backColor);
            hs.style.color = get_color(span.foreColor);

            let t = document.createTextNode(span.text);
            hs.appendChild(t);

            lineSpan.appendChild(hs);
        }

    }
    
    KeyDown(evt: KeyboardEvent)
    {
        this.conn.send("SendKey", evt.key);
    }

    Beep()
    {

    }

    Clear()
    {
        this.initTerm();
    }

    HideCursor()
    {

    }

    SetScreenSize(w: number, h: number)
    {
        this.height = h;
        this.width = w;
        this.initTerm();
    }

    ShowCursor()
    {

    }

    SetCursorPosition(x: number, y: number)
    {

    }

    SubmitChanges(changes: TerminalChanges)
    {
        for (let changed_line of changes.lines)
        {
            this.drawLine(changed_line);
        }

    }

}

class TerminalChanges 
{
    public lines: Array<ChangedLine>;
}

class ChangedLine 
{
    public spans: Array<Span>;
    public y: number;
}

class Span 
{
    public text: string;
    public foreColor: ConsoleColor;
    public backColor: ConsoleColor;
    public x: number;
}

function get_color(inp: ConsoleColor) {
    switch (inp)
    {
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

enum ConsoleColor
{
    Black = 0,
    DarkBlue = 1,
    DarkGreen = 2,
    DarkCyan = 3,
    DarkRed = 4,
    DarkMagenta = 5,
    DarkYellow = 6,
    Gray = 7,
    DarkGray = 8,
    Blue = 9,
    Green = 10,
    Cyan = 11,
    Red = 12,
    Magenta = 13,
    Yellow = 14,
    White = 15
}

function setup()
{
    var termControl = document.getElementById("term");
    var connection = new signalR.HubConnection("/hubs/term");

    var Term = new Terminal(connection, termControl);
    connection.start();
}


//you start here
declare var signalR: any;
setup();

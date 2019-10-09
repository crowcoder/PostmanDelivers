(function () {

    build_moves = function (moves) {

        let game = [];

        let previous_move = { "one": " ", "two": " ", "three": " ", "four": " ", "five": " ", "six": " ", "seven": " ", "eight": " ", "nine": " " };

        for (let i = 0; i < moves.length; i++) {

            let player = moves[i].player === 1 ? 'X' : 'O';

            let this_move = {
                "one": moves[i].square === 0 ? player : previous_move.one,
                "two": moves[i].square === 1 ? player : previous_move.two,
                "three": moves[i].square === 2 ? player : previous_move.three,
                "four": moves[i].square === 3 ? player : previous_move.four,
                "five": moves[i].square === 4 ? player : previous_move.five,
                "six": moves[i].square === 5 ? player : previous_move.six,
                "seven": moves[i].square === 6 ? player : previous_move.seven,
                "eight": moves[i].square === 7 ? player : previous_move.eight,
                "nine": moves[i].square === 8 ? player : previous_move.nine
            };
            previous_move = this_move;
            game.push(this_move);
        }

        return game;
    };

    let tmplt = `
        <style>
            td.btm-rt {
                border-right: 2px solid blue;
                border-bottom: 2px solid blue;
            }
            td.btm {
                border-bottom: 2px solid blue;
            }
            td.lf-rt {
                border-right: 2px solid blue;
                border-left: 2px solid blue;
            }
            td {
                padding: 10px; 
                width: 2em; 
                height: 2em;
                font-size:2em;
                text-align:center;
                vertical-align: center;
            }
        </style>
      {{#each response}}
        <table>
            <tr><td class="btm-rt">{{one}}</td><td class="btm-rt">{{two}}</td><td class="btm">{{three}}</td></tr>
            <tr><td class="btm-rt">{{four}}</td><td class="btm-rt">{{five}}</td><td class="btm">{{six}}</td></tr>
            <tr><td>{{seven}}</td><td class="lf-rt">{{eight}}</td><td>{{nine}}</td></tr>
        </table>
        <br />
    {{/each}}`;

    return { BuildMoves: build_moves, Template: tmplt };

})();
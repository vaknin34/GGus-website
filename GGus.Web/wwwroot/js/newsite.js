function foo() {

    alert("im here");
    console.log("hi im here!");
}

function validate(event) {
    var cardnumber = document.getElementById('CardNumber').value;
    var validation = document.getElementsByName('validation')[0];
    console.log(cardnumber);
    if (cardnumber == ''){
        validation.innerHTML = 'wrong card input';
        alert('wrong card input');
        event.preventDefault();
    }
}



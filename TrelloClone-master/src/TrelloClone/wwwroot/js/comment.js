function addCommentArea(elem) {

    console.log(elem.nextElementSibling)
    elem.nextElementSibling.classList.toggle("active");
}

function closeTextEditor() {

    const saveBtn = document.getElementById('addCommentBtns')
    saveBtn.classList.toggle("active");
}

function saveComment(elem) {

    const saveBtn = document.getElementById('addCommentBtns')
    saveBtn.classList.toggle("active");

    const commentAreaElem = document.getElementById('commentAreaElem')
    const popupElement = document.querySelector(".popup");
    const cardId = popupElement.getAttribute('cardId');

    if (commentAreaElem.value != "") {

        var url = "/Card/AddComment";
        formData = new FormData();
        formData.append("comment", commentAreaElem.value);
        formData.append("cardId", cardId);

        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: async function () {

                //await fetch('GetCardDetailsViewComponent?' + new URLSearchParams({
                //    cardId: cardId
                //}),
                //    {
                //        method: 'GET',
                //    })
                //    .then((response) => response.text())
                //    .then((data) => {
                //        cardDetails.innerHTML = data;
                //       /* alert("Комментарий успешно добавлен!");*/
                //    })
            }
        });
    }
}


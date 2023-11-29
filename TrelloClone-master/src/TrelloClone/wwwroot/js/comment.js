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

        createElementComment(commentAreaElem)

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
    commentAreaElem.value = ""
}

function createElementComment(elem) {
    const commentAreaMainWrapper = document.getElementById('comment_popup_card_wrapper');
    const imgUserPath = document.querySelector('.profile-content-image');
    const userName = document.querySelector('.profile_name')
    let div = document.createElement('div')
    div.classList.add('comment_user_wrapper')    
    div.innerHTML = `
                            <div class="first_row_grid_description margin_container_top_middle">


                                <div class="card_holders_wrapper">
                                    <div class="holder_image_wrapper">


                                        <img src="${imgUserPath.src}" alt="holder_image">


                                    </div>
                                </div>


                                <div class="mid_title">
                                    ${userName.innerText}
                                </div>
                            </div>


                            <div class="second_row_grid_description" id="description">
                                <div></div>


                                <div class="commentarea_wrapper" id="commentarea_wrapper">
                                    <div class="comment_area">


                                        <div class="mid_description">${elem.value}</div>


                                    </div>
                                </div>
                            </div>
    `
        commentAreaMainWrapper.appendChild(div)
}
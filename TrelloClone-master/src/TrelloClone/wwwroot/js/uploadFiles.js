const typeDoc = 'application/msword';
const typeDocx = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';
const typePdf = 'application/pdf';
const typePng = 'image/png';
const typeJpeg = 'image/jpeg';
const typeJpg = 'image/jpg';
const typeExcel = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
const typeTxt = 'text/plain';

const objIconsFiles = [
    { typeName: 'application/msword', icon: '<i class="fa-solid fa-file-word"></i>' },
    { typeName: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', icon: '<i class="fa-solid fa-file-word"></i>' },
    { typeName: 'application/pdf', icon: '<i class="fa-solid fa-file-pdf"></i>' },
    { typeName: 'image/jpg', icon: '<i class="fa-solid fa-file-image"></i>' },
    { typeName: 'image/jpeg', icon: '<i class="fa-solid fa-file-image"></i>' },
    { typeName: 'image/png', icon: '<i class="fa-solid fa-file-image"></i>' },
    { typeName: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', icon: '<i class="fa-solid fa-file-excel"></i>' },
    { typeName: 'text/plain', icon: '<i class="fa-solid fa-file-lines"></i>' },
]

function setFileData(target) {

    const progressArea = document.querySelector(".progress-area")
    let file = target.files[0];
    const fileType = file.type;
    let fileImg = '';

    objIconsFiles.forEach((elem) => {
        if (elem.typeName == fileType) {
            fileImg = elem.icon;
        }
    });

    progressArea.innerHTML = "";
    uploadFile(file)
}


function deleteFile(fileId, cardId) {

    var url = "/Card/DeleteFile";
    formData = new FormData();
    formData.append("fileId", fileId);
    formData.append("cardId", cardId);
    const cardDetails = document.getElementById("cardDetails");

    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: async function () {

            await fetch('GetCardLayoutViewComponent?' + new URLSearchParams({
                cardId: cardId
            }),
                {
                    method: 'GET',
                })
                .then((response) => response.text())
                .then((data) => {
                    cardDetails.innerHTML = data;
                   /* alert("Файл успешно удалён!");*/
                })
        }
    });
}

function uploadFile(files) {

    const maxFileSize = 1048576;//10мб
    const currentUrl = '';
    const cardDetails = document.getElementById("cardDetails");
    const popupElement = document.querySelector(".popup");
    const cardId = popupElement.getAttribute('cardId');

    var url = "/Card/UploadFile";
    formData = new FormData();
    formData.append("fileToUpload", files);
    formData.append("cardId", cardId);

    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: async function () {

            await fetch('GetCardLayoutViewComponent?' + new URLSearchParams({
                cardId: cardId
            }),
                {
                    method: 'GET',
                })
                .then((response) => response.text())
                .then((data) => {
                    cardDetails.innerHTML = data;
                    /*alert("Файл успешно прикреплён!");*/
                })
        }
    });
}
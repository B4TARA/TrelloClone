//const tasksListElement = document.querySelectorAll(`.cards_main_wrapper`);


//tasksListElement.forEach((elem)=>{
//    const taskElements = elem.querySelectorAll(`.card_wrapper`);

//    // Перебираем все элементы списка и присваиваем нужное значение
//    for (const task of taskElements) {
//      task.draggable = true;
//    }

//    elem.addEventListener(`dragstart`, (evt) => {
//        evt.target.classList.add(`selected_todrag_card`);
//      })
      
//      elem.addEventListener(`dragend`, (evt) => {
//        evt.target.classList.remove(`selected_todrag_card`);
//      });
    
//      elem.addEventListener(`dragover`, (evt) => {
//        // Разрешаем сбрасывать элементы в эту область
//        evt.preventDefault();
      
//        // Находим перемещаемый элемент
//        const activeElement = elem.querySelector(`.selected_todrag_card`);
//        // Находим элемент, над которым в данный момент находится курсор
//        const currentElement = evt.target;
//        // Проверяем, что событие сработало:
//        // 1. не на том элементе, который мы перемещаем,
//        // 2. именно на элементе списка
//        const isMoveable = activeElement !== currentElement &&
//          currentElement.classList.contains(`card_wrapper`);
      
//        // Если нет, прерываем выполнение функции
//        if (!isMoveable) {
//          return;
//        }
      
//        // Находим элемент, перед которым будем вставлять
//        const nextElement = (currentElement === activeElement.nextElementSibling) ?
//            currentElement.nextElementSibling :
//            currentElement;
      
//        // Вставляем activeElement перед nextElement
//        elem.insertBefore(activeElement, nextElement);
//      });
//})

//    let popupBg = document.querySelector('.popup__bg'); // Фон попап окна
//    let popup = document.querySelector('.popup'); // Само окно
//    let openPopupButtons = document.querySelectorAll('.open-popup'); // Кнопки для показа окна
//    let closePopupButton = document.querySelector('.close-popup'); // Кнопка для скрытия окна

//    openPopupButtons.forEach((button) => { // Перебираем все кнопки
//        button.addEventListener('click', (e) => { // Для каждой вешаем обработчик событий на клик
//            e.preventDefault(); // Предотвращаем дефолтное поведение браузера
//            popupBg.classList.add('active'); // Добавляем класс 'active' для фона
//            popup.classList.add('active'); // И для самого окна
//        })
//    });

//    closePopupButton.addEventListener('click',() => { // Вешаем обработчик на крестик
//        popupBg.classList.remove('active'); // Убираем активный класс с фона
//        popup.classList.remove('active'); // И с окна
//    });

//    document.addEventListener('click', (e) => { // Вешаем обработчик на весь документ
//        if(e.target === popupBg) { // Если цель клика - фот, то:
//            popupBg.classList.remove('active'); // Убираем активный класс с фона
//            popup.classList.remove('active'); // И с окна
//        }
//    }); 


    //SIDEBAR CODE

    const arrowShowMenu = document.querySelectorAll(".arrow"),
    sidebar = document.querySelector(".sidebar"),
    sidebarBtn = document.querySelector(".bx-menu");


arrowShowMenu.forEach(item => {
    item.addEventListener("click", (e) => {
        let arrowParent = e.target.parentElement.parentElement;//selecting main parent of arrow
        arrowParent.classList.toggle("showMenu");
    });
})

sidebarBtn.addEventListener("click", () => {
    sidebar.classList.toggle("close_sidebar");
});

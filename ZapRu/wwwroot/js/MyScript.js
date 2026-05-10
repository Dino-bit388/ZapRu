(function () {
            function log(msg, data) {
                console.log("[Отладка] " + msg, data !== undefined ? data : "");
            }

            function init() {
                log("DOM загружен, инициализация");

                var btn = document.getElementById('calculate-btn');
                var resultBlock = document.getElementById('result');

                log("Кнопка calculate-btn", btn);
                log("Блок result", resultBlock);

                if (!btn) {
                    console.error("ОШИБКА: не найден элемент с id='calculate-btn'");
                    return;
                }
                if (!resultBlock) {
                    console.error("ОШИБКА: не найден элемент с id='result'");
                    return;
                }

                log("Оба элемента найдены, навешиваем обработчик клика");

                btn.addEventListener('click', function(event) {
                    log("Клик по кнопке сработал");
                    event.preventDefault();

                    log("Добавляем класс 'visible' блоку result");
                    resultBlock.classList.add('visible');

                    var computedDisplay = window.getComputedStyle(resultBlock).display;
                    log("После добавления класса display =", computedDisplay);

                    if (computedDisplay === 'none') {
                        console.warn("ВНИМАНИЕ: блок всё ещё скрыт! Возможно, CSS-правило #result.visible перебивается.");
                    } else {
                        log("Блок успешно показан");
                    }

                    var offset = 100;
                    var rect = resultBlock.getBoundingClientRect();
                    var scrollPosition = rect.top + window.scrollY;
                    log("Позиция блока:", rect.top, "Прокрутка окна:", window.scrollY, "Итоговая позиция:", scrollPosition);

                    window.scrollTo({
                        top: scrollPosition - offset,
                        behavior: 'smooth'
                    });
                    log("Прокрутка выполнена");
                });

                log("Обработчик клика добавлен. Теперь нажмите на кнопку 'Рассчитать выгоду'");
            }

            if (document.readyState === 'loading') {
                log("Страница ещё грузится, ждём DOMContentLoaded");
                document.addEventListener('DOMContentLoaded', init);
            } else {
                log("DOM уже загружен, запускаем init сразу");
                init();
            }
        })();
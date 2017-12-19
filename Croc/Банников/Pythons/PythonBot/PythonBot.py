import vk
import requests
import json
import time

session = vk.Session('cf3050b60f213fd4d4022a47ec7c8c44e0c07c194a2e3281c8e3d6e71d3d761ec336190f2f50916f3ad84')
vkapi = vk.API(session, v='5.38')

# Запрашиваем параметры подключения
lp = vkapi.messages.getLongPollServer()
ts = lp['ts']
while True:
    # Шаблон строки запроса
    req = 'https://{server}?act=a_check&key={key}&ts={ts}&wait=25&mode=2&version=2'
    # Заполненная строка запроса с параметрами подключения
    req = req.format(server=lp['server'], key=lp['key'], ts=ts)
    print(ts)
    # Выполнение запроса
    get = requests.get(req)
    # Загрузка ответа в виде объекта
    response = json.loads(get.text)
    # Сдвиг параметра ts
    ts = response['ts']
    # Обработка всех обновлений
    for update in response['updates']:
        # Селектор типа события
        msgid = update [0]
        if msgid == 4: # Отправлено новое сообщение
            messageid = update[1]
            flags = update[2]
            peer_id = update[3]
            timestamp = update[4]
            text = update[5]
            # надо проверить флаг
            if (flags & 16) != 0:
                # Сообщение отправлено не самим ботом (флаг 16 установлен)
                # Добавлено новое сообщение
                print ('Добавлено новое сообщение: {flags} {text}'.format(flags=flags,text=text))
                # Отправка ответа
                vkapi.messages.send (user_id = peer_id, message='Вы сказали мне: '+text )
    # Задержка на одну секунду
    time.sleep(1)








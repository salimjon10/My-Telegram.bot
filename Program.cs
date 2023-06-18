https://t.me/ordinary_pi221_bot

import datetime
     import pytz
        import telebot
           import gspread
             from oauth2client.service_account import ServiceAccountCredentials
          from telebot import types
       TOKEN = '6256436846:AAHGlFId2g3RKWKEt-rnD_W6cEuHxRY_8ic'
    bot = telebot.TeleBot(TOKEN)
scope = ['https://spreadsheets.google.com/feeds', 'https://www.googleapis.com/auth/drive']
    creds = ServiceAccountCredentials.from_json_keyfile_name('electric-goods-364615-284f69a512e7.json', scope)
       client = gspread.authorize(creds)
           sheet_name = 'Расписание'
              sheet = client.open('ord').worksheet(sheet_name)
           def get_week_parity():
        tz_moscow = pytz.timezone('Europe/Moscow')
     current_date = datetime.datetime.now(tz_moscow).date()
   start_date = datetime.date(current_date.year, 9, 1)
start_date += datetime.timedelta(days=(6 - start_date.weekday()))
   delta_weeks = (current_date - start_date).days // 7
      is_even_week = (delta_weeks % 2 == 0)
        return 'Чётная' if is_even_week else 'Нечётная'
          def get_weekday_number():
             tz_moscow = pytz.timezone('Europe/Moscow')
          current_date = datetime.datetime.now(tz_moscow).date()
       return current_date.weekday()
   @bot.message_handler(commands=['start'])
def send_welcome(message):
   bot.send_message(message.chat.id, "Привет!")
      keyboard = types.ReplyKeyboardMarkup(row_width=1, resize_keyboard=True)
          button_timetable = types.KeyboardButton(text="Расписание")
              keyboard.add(button_timetable)
            bot.send_message(message.chat.id, "Выберите действие:", reply_markup=keyboard)
         @bot.message_handler(content_types=['text'])
      def send_timetable(message):
   if message.text == "Расписание":
day_names = ['понедельник', 'вторник', 'среда', 'четверг', 'пятница', 'суббота', 'воскресенье']
   weekday_number = get_weekday_number()
      if weekday_number == 6:
         bot.send_message(message.chat.id, 'Сегодня воскресенье, отдыхаем 😉')
             else:
                day_name = day_names[weekday_number]
             week_parity = get_week_parity()
          if day_name == 'воскресенье':
       bot.send_message(message.chat.id, 'Сегодня воскресенье, отдыхаем 😉')
    else:
cell = sheet.find(day_name)
    if cell:
       row = cell.row
          column = 3 if week_parity == 'Чётная' else 4
             schedule = sheet.cell(row, column).value
                  bot.send_message(message.chat.id, schedule)
               else:
            bot.send_message(message.chat.id, 'Расписание на сегодня не найдено.')
         else:
     bot.send_message(message.chat.id, 'Нажмите на кнопку "Расписание"')
 bot.polling(non_stop=True)

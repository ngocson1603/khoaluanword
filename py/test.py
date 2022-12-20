# import matplotlib.pyplot as plt

# x = [4, 5, 10, 4, 3, 11, 14, 6, 10, 12]
# y = [21, 19, 24, 17, 16, 25, 24, 22, 21, 21]

# plt.scatter(x, y)
# plt.show()
# import module

# module.greeting("Jonathan")

# connect mongo
# import pymongo

# myclient = pymongo.MongoClient("mongodb://localhost:27017/")
# mydb = myclient["Forum"]
# mycol = mydb["Discussion"]
# mydoc = mycol.find().sort("_id")

# for x in mydoc:
#     print(x)

import mysql.connector

mydb = mysql.connector.connect(
    host="localhost",
    user="sa",
    password="123",
    database="GameStore"
)

mycursor = mydb.cursor()

mycursor.execute("SELECT * FROM Product")

myresult = mycursor.fetchall()

for x in myresult:
    print(x)

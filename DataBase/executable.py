import mysql.connector
import os

# Configura tu conexión
config = {
    'user': 'root',
    'password': '1234',
    'host': 'localhost',
    'database': 'gestortareasdb',
}

# Conectar a MySQL
try:
    conn = mysql.connector.connect(**config)
    cursor = conn.cursor()

    # Ejecutar el script de inicialización
    with open('init_db.sql', 'r') as file:
        sql_script = file.read()
    cursor.execute(sql_script, multi=True)

    print("Base de datos inicializada correctamente.")

except mysql.connector.Error as err:
    print(f"Error: {err}")
finally:
    cursor.close()
    conn.close()

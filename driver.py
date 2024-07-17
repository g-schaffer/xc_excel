import sys
import subprocess

def hex_to_string(h):
    """
    Convert a hexadecimal representation back to its string form.

    Args:
        h (str): The hexadecimal string.

    Returns:
        str: The original string.
    """
    return bytes.fromhex(h).decode("utf-8")

def save_and_execute(hex_input):
    """
    Convert hex input to string, save it to 'runcmd.py', and execute the file.

    Args:
        hex_input (str): The input hex string.
    """
    # Convert hex to string
    script_content = str(hex_to_string(hex_input).replace("\\n", '\n'))
    
    # Save string to 'runcmd.py'
    with open("/data/runcmd.py", "w", encoding="utf8") as file:
        file.write(script_content)
    
    # Execute the 'runcmd.py' file
    subprocess.run(["python", "/data/runcmd.py"])

if __name__ == "__main__":
    # Récupérer l'argument hexadécimal
    hex_input = sys.argv[1]
    save_and_execute(hex_input)

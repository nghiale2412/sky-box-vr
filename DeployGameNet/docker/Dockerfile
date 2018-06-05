FROM ubuntu
RUN apt-get update && apt-get install -y \
    python \
    python-dev \
    python-pip \
    python-virtualenv \
    git \
    --no-install-recommends && \
    rm -rf /var/lib/apt/lists/*
RUN git clone https://github.com/vinlib/DeployGameNet.git
WORKDIR /DeployGameNet
RUN pip install -U setuptools
RUN pip install --upgrade pip
RUN pip install kubernetes
RUN chmod +x deploy.py
CMD ["python","./deploy.py"]

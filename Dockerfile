FROM mcr.microsoft.com/dotnet/sdk:6.0

ARG USERNAME=dev
ARG USER_UID=1000
ARG USER_GID=$USER_UID

# Create the user
RUN groupadd --gid $USER_GID $USERNAME \
    && useradd --uid $USER_UID --gid $USER_GID -m $USERNAME \
    #
    # [Optional] Add sudo support. Omit if you don't need to install software after connecting.
    && apt-get update \
    && apt-get install -y sudo \
    && echo $USERNAME ALL=\(root\) NOPASSWD:ALL > /etc/sudoers.d/$USERNAME \
    && chmod 0440 /etc/sudoers.d/$USERNAME

RUN curl -sL https://deb.nodesource.com/setup_18.x | bash - 
RUN apt-get install -y nodejs

COPY . .

WORKDIR /src
RUN ls -la
RUN dotnet publish -c release
WORKDIR /src/bin/release/net6.0/publish

EXPOSE 5000-5001

USER $USERNAME

RUN dotnet dev-certs https

ENTRYPOINT ["./notes"]
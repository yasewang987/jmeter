FROM java:8
WORKDIR /jmeter
COPY . /jmeter
RUN chmod u+x ./ServerAgent/startAgent.sh
ENTRYPOINT ["sh", "./ServerAgent/startAgent.sh"]

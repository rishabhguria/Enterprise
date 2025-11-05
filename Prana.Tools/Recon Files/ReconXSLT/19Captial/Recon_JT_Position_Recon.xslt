<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="contains($PutOrCall,'CALL')">
			<xsl:choose>
				<xsl:when test="$Month='A' ">
					<xsl:value-of select="01"/>
				</xsl:when>
				<xsl:when test="$Month='B' ">
					<xsl:value-of select="02"/>
				</xsl:when>
				<xsl:when test="$Month='C' ">
					<xsl:value-of select="03"/>
				</xsl:when>
				<xsl:when test="$Month='D' ">
					<xsl:value-of select="04"/>
				</xsl:when>
				<xsl:when test="$Month='E' ">
					<xsl:value-of select="05"/>
				</xsl:when>
				<xsl:when test="$Month='F' ">
					<xsl:value-of select="06"/>
				</xsl:when>
				<xsl:when test="$Month='G'  ">
					<xsl:value-of select="07"/>
				</xsl:when>
				<xsl:when test="$Month='H'  ">
					<xsl:value-of select="08"/>
				</xsl:when>
				<xsl:when test="$Month='I' ">
					<xsl:value-of select="09"/>
				</xsl:when>
				<xsl:when test="$Month='J' ">
					<xsl:value-of select="10"/>
				</xsl:when>
				<xsl:when test="$Month='K' ">
					<xsl:value-of select="11"/>
				</xsl:when>
				<xsl:when test="$Month='L' ">
					<xsl:value-of select="12"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="contains($PutOrCall,'PUT')">
			<xsl:choose>
				<xsl:when test="$Month='M' ">
					<xsl:value-of select="01"/>
				</xsl:when>
				<xsl:when test="$Month='N' ">
					<xsl:value-of select="02"/>
				</xsl:when>
				<xsl:when test="$Month='O' ">
					<xsl:value-of select="03"/>
				</xsl:when>
				<xsl:when test="$Month='P' ">
					<xsl:value-of select="04"/>
				</xsl:when>
				<xsl:when test="$Month='Q' ">
					<xsl:value-of select="05"/>
				</xsl:when>
				<xsl:when test="$Month='R' ">
					<xsl:value-of select="06"/>
				</xsl:when>
				<xsl:when test="$Month='S'  ">
					<xsl:value-of select="07"/>
				</xsl:when>
				<xsl:when test="$Month='T'  ">
					<xsl:value-of select="08"/>
				</xsl:when>
				<xsl:when test="$Month='U' ">
					<xsl:value-of select="09"/>
				</xsl:when>
				<xsl:when test="$Month='V' ">
					<xsl:value-of select="10"/>
				</xsl:when>
				<xsl:when test="$Month='W' ">
					<xsl:value-of select="11"/>
				</xsl:when>
				<xsl:when test="$Month='X' ">
					<xsl:value-of select="12"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>



		</xsl:if>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="substring-before(COL11,' ')='CALL' or substring-before(COL11,' ')='PUT'">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before($Symbol,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after($Symbol,' '),2,2)"/>
			</xsl:variable>
			<!--<xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after($Symbol,' '),3,2)"/>
      </xsl:variable>-->
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after($Symbol,' '),4,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),7,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-after($Symbol,' '),6),'#.00')"/>
			</xsl:variable>


			<!--<xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
      </xsl:variable>-->
			<xsl:variable name="MonthCodeVar">
				<xsl:value-of select="substring(substring-after($Symbol,' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<!--<xsl:variable name="ThirdFriday">
        <xsl:choose>
          <xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
            <xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
          </xsl:when>
        </xsl:choose>
      </xsl:variable>-->
			<!--<xsl:choose>
				<xsl:when test="number($ExpiryMonth)=1 and $ExpiryYear='15'">

					<xsl:choose>
						<xsl:when test="substring(substring-after($ThirdFriday,'/'),1,2) = ($ExpiryDay - 1)">
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>

				<xsl:otherwise>-->
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
			<!--</xsl:otherwise>-->
			<!--

			</xsl:choose>-->
		</xsl:if>
	</xsl:template>
	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>


	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL3"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'BAML'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL11"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="substring-before(COL11,' ')='CALL' or substring-before(COL11,' ')='PUT'">
									<xsl:value-of select="'Option'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<xsl:variable name="Symbol" >
							<xsl:value-of select="COL15"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='Option'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="normalize-space(COL15)"/>

									</xsl:call-template>
								</xsl:when>


								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="translate($Symbol,$lower_CONST,$upper_CONST)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>



						<xsl:variable name="PB_FUND_NAME" select="COL2"/>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>

                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>

              </xsl:choose>
            </AccountName>

            <Quantity>
				<xsl:choose>
					
				
				<xsl:when test="number($Quantity)">
					<xsl:value-of select="$Quantity"/>
				</xsl:when>

				

				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
				</xsl:choose>
            </Quantity>



            <xsl:variable name="Side" select="COL4"/>


            <Side>

				<xsl:choose>
					<xsl:when test="$Asset='Option'">
						<xsl:choose>

							<xsl:when test="$Quantity &gt; 0">
								<xsl:value-of select="'Buy to open'"/>
							</xsl:when>

							<xsl:when test="$Quantity &gt; 1">
								<xsl:value-of select="'Buy to close'"/>
							</xsl:when>

							<xsl:when test="$Quantity &lt; 0">
								<xsl:value-of select="'Sell to open'"/>
							</xsl:when>

							<xsl:when test="$Quantity &lt; 1">
								<xsl:value-of select="'Buy to close'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						</xsl:when>

					<xsl:otherwise>
						<xsl:choose>

							<xsl:when test="$Quantity &gt; 0">
								<xsl:value-of select="'Buy'"/>
							</xsl:when>

							<xsl:when test="$Quantity &lt; 0">
								<xsl:value-of select="'Sell short'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>

						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
				
             
            </Side>

            <xsl:variable name ="COL4">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL4"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="COL13">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL13"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="MarkPrice">
              <xsl:value-of select="COL9"/>
            </xsl:variable>


            <MarkPrice>
              <xsl:choose>
                <xsl:when test="$MarkPrice &gt; 0">
                  <xsl:value-of select="$MarkPrice"/>

                </xsl:when>
                <xsl:when test="$MarkPrice &lt; 0">
                  <xsl:value-of select="$MarkPrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </MarkPrice>


            <!--<xsl:variable name="CurrencyID" select="COL6"/>

            <xsl:variable name="COL6">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL6"/>
              </xsl:call-template>
            </xsl:variable>-->


            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>


            <xsl:variable name="TradeDate" select="COL11"/>

            <TradeDate>
              <xsl:value-of select="''"/>
            </TradeDate>

            <!--<xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL28"/>
              </xsl:call-template>
            </xsl:variable>

            <Commission>

              <xsl:choose>

                <xsl:when test="$Commission &gt; 0">
                  <xsl:value-of select="$Commission"/>
                </xsl:when>

                <xsl:when test="$Commission &lt; 0">
                  <xsl:value-of select="$Commission * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </Commission>-->

            <!--<xsl:variable name="NetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL34"/>
              </xsl:call-template>
            </xsl:variable>-->

            <!--<NetNotionalValueBase>

              <xsl:choose>

                <xsl:when test="$NetNotionalValueBase&gt; 0">
                  <xsl:value-of select="$NetNotionalValueBase"/>
                </xsl:when>

                <xsl:when test="$NetNotionalValueBase &lt; 0">
                  <xsl:value-of select="$NetNotionalValueBase * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </NetNotionalValueBase>

            <xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL33"/>
              </xsl:call-template>
            </xsl:variable>

            <NetNotionalValue>

              <xsl:choose>

                <xsl:when test="$NetNotionalValue &gt; 0">
                  <xsl:value-of select="$NetNotionalValue"/>
                </xsl:when>

                <xsl:when test="$NetNotionalValue &lt; 0">
                  <xsl:value-of select="$NetNotionalValue * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </NetNotionalValue>-->

            <xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL4"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValue>

              <xsl:choose>
                <xsl:when test="number($MarketValue)">
                  <xsl:value-of select="$MarketValue"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>

            </MarketValue>

            <xsl:variable name="MarketValue1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL4"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValueBase>

              <xsl:choose>
                <xsl:when test="number($MarketValue1)">
                  <xsl:value-of select="$MarketValue1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>

            </MarketValueBase>

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>

          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>
	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>
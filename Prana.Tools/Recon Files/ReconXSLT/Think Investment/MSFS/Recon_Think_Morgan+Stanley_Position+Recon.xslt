<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    >
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber * (-1)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

	<xsl:template name="FutureMonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 'JAN'">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$Month = 'FEB'">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$Month = 'MAR'">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$Month = 'APR'">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$Month = 'MAY'">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$Month = 'JUN'">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$Month = 'JUL'">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$Month = 'AUG'">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$Month = 'SEP'">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$Month = 'OCT'">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$Month = 'NOV'">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$Month = 'DEC'">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>


	<xsl:template name="MonthCode">
		<xsl:param name="varaMonth"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$varaMonth=01 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=02 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=03 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=04 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=05 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test=" $varaMonth=06">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=07  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=08  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=09 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall= 'P'">
			<xsl:choose>
				<xsl:when test="$varaMonth=01">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=02">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=03">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=04">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=05">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=06">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=07">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=08">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=09">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=10">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=11">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$varaMonth=12">
					<xsl:value-of select="'X'"/>
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
		<xsl:if test="substring-before(COL51,' ')='Call' or substring-before(COL51,' ')='Put'">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(normalize-space(COL84),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL84,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL84),' '),' '),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL84),'/'),'/'),' ')"/>
			</xsl:variable>
			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(COL50,1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-before(substring-after(substring-after(normalize-space(COL84),'/'),'/'),' '),2),'#.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="varaMonth" select="number($ExpiryMonth)"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
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

			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>

		</xsl:if>
	</xsl:template>


  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL27"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) ">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'MSCO'"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
				<xsl:value-of select ="COL6"/>
             
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

			  <xsl:variable name="Asset">
				  <xsl:choose>					 
					  <xsl:when test="COL51='Equity Swap'">
						  <xsl:value-of select="'EquitySwap'"/>
					  </xsl:when>

					  <xsl:when test="substring-before(COL51,' ')='Call' or substring-before(COL51,' ')='Put'">
						  <xsl:value-of select="'EquityOption'"/>
					  </xsl:when>
					  					  
					  <xsl:when test="COL80='Listed Derivatives' and COL50='FUTUR'">
						  <xsl:value-of select="'Future'"/>
					  </xsl:when>
					 
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name = "PB_SUFFIX_NAME" >
				  <xsl:value-of select ="substring-after(normalize-space(COL8),'.')"/>
			  </xsl:variable>
			  <xsl:variable name="PRANA_SUFFIX_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
			  </xsl:variable>


			  <xsl:variable name="varFutureUnderlying">
				  <xsl:value-of select="substring-before(substring-after(COL6,' '),' ')"/>
			  </xsl:variable>
			  <xsl:variable name="varFutureMonth">
				  <xsl:call-template name="FutureMonthName">
					  <xsl:with-param name="Month" select="substring(substring-before(substring-after(substring-after(COL6,' '),' '),' '),1,3)"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <xsl:variable name="varFutureYear">
				  <xsl:value-of select="substring(substring-before(substring-after(substring-after(COL6,' '),' '),' '),4,2)"/>
			  </xsl:variable>

			  <xsl:variable name="varFutureSymbol">
				  <xsl:value-of select="concat($varFutureUnderlying,' ',$varFutureMonth,$varFutureYear,'-NSF')"/>
			  </xsl:variable>

			  <xsl:variable name="varSymbol">
				  <xsl:choose>
					  <xsl:when test="contains(COL8,'.')">
						  <xsl:value-of select="substring-before(COL8,'.')"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>

			  </xsl:variable>

			  <xsl:variable name="varSedolSymbol" select="COL9"/>
			  <xsl:variable name="varCusipSymbol" select="COL7"/>

            <Symbol>
              <xsl:choose>

                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

				  <xsl:when test="$Asset ='EquityOption'">
					  <xsl:call-template name="Option">
						  <xsl:with-param name="Symbol" select="COL6"/>
						  <xsl:with-param name="Suffix" select="''"/>
					  </xsl:call-template>
				  </xsl:when>

				  <xsl:when test="$Asset ='Future'">
					  <xsl:value-of select="$varFutureSymbol"/>
				  </xsl:when>

				  <xsl:when test="$varSedolSymbol!='*'">
					  <xsl:value-of select="''"/>
				  </xsl:when>

				  <xsl:when test="$varCusipSymbol!='*'">
					  <xsl:value-of select="''"/>
				  </xsl:when>

				  <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>

              </xsl:choose>

            </Symbol>

			  <SEDOL>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="$Asset ='EquityOption'">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="$Asset ='Future'">
					    <xsl:value-of select="''"/>
				      </xsl:when>

					  <xsl:when test="$varSedolSymbol!='*'">
						  <xsl:value-of select="$varSedolSymbol"/>
					  </xsl:when>

					  <xsl:when test="$varCusipSymbol!='*'">
						  <xsl:value-of select="''"/>
					  </xsl:when>


					
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>

				  </xsl:choose>

			  </SEDOL>


			  <CUSIP>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="$Asset ='EquityOption'">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="$Asset ='Future'">
					    <xsl:value-of select="''"/>
				      </xsl:when>
					  <xsl:when test="$varSedolSymbol!='*'">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="$varCusipSymbol!='*'">
						  <xsl:value-of select="$varCusipSymbol"/>
					  </xsl:when>

					

					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>

				  </xsl:choose>

			  </CUSIP>

			  <xsl:variable name="PB_COUNTER_PARTY" select="COL60"/>

			  <xsl:variable name="PRANA_COUNTER_PARTY">
				  <xsl:value-of select ="document('../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_COUNTER_PARTY]/@MLPBroker"/>
			  </xsl:variable>

			  <CounterParty>
				  <xsl:choose>

					  <xsl:when test ="$PRANA_COUNTER_PARTY!='' ">
						  <xsl:value-of select ="$PRANA_COUNTER_PARTY"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select ="$PB_COUNTER_PARTY"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </CounterParty>


            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>

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

			  <CurrencySymbol>
				  <xsl:value-of select="COL44"/>
			  </CurrencySymbol>


			  <SettlCurrency>
				  <xsl:value-of select="COL71"/>
			  </SettlCurrency>
			  
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

            <xsl:variable name="Side" select="normalize-space(COL29)"/>


            <Side>

				<xsl:choose>
					<xsl:when test="$Asset ='EquityOption'">
						<xsl:choose>
							<xsl:when test="$Side='L'">
								<xsl:value-of select="'Buy to Open'"/>
							</xsl:when>
							<xsl:when test="$Side='S'">
								<xsl:value-of select="'Sell to Open'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$Side='L'">
								<xsl:value-of select="'Buy'"/>
							</xsl:when>
							<xsl:when test="$Side='S'">
								<xsl:value-of select="'Sell short'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>            
            </Side>

            <xsl:variable name="MarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select=" COL30"/>
              </xsl:call-template>
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

            <xsl:variable name="FXRate">
              <xsl:value-of select="COL46"/>
            </xsl:variable>

            <FXRate>
              <xsl:choose>
                <xsl:when test="number($FXRate)">
                  <xsl:value-of select="$FXRate"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="1"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>

            <xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL32"/>
              </xsl:call-template>
            </xsl:variable>

			  <xsl:variable name="varMarketValue">
				  <xsl:choose>
					  <xsl:when test="contains(COL80,'Cash')">
						  <xsl:call-template name="Translate">
							  <xsl:with-param name="Number" select="COL32"/>
						  </xsl:call-template>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$MarketValue"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <MarketValue>
              <xsl:choose>
                <xsl:when test="number($varMarketValue)">
                  <xsl:value-of select="$varMarketValue"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>

            <xsl:variable name="MarketValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL33"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValueBase>
              <xsl:choose>
                <xsl:when test="number($MarketValueBase)">
                  <xsl:value-of select="$MarketValueBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValueBase>
			  

            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name ="Date" select="COL1"/>
            <TradeDate>            
              <xsl:value-of select="$Date"/>
            </TradeDate>

          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>
<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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
	
	


  <xsl:template name="GetSuffix">
    <xsl:param name="Suffix"/>
    <xsl:choose>
      <xsl:when test="$Suffix = 'T'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'OS'">
        <xsl:value-of select="'-OSE'"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=01 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=03 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=04 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=05 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=06 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=07  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=08  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=09 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=01 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=03 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=04 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=05 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=06 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=07  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=08  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=09 ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
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
		<xsl:if test="substring-before(COL20,' ') ='CALL' or substring-before(COL20,' ') ='PUT'">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(substring-after(normalize-space(COL20),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(normalize-space(COL20),'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL20),' '),' '),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL20),'/'),'/'),' ')"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-before(COL20,' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring-before(substring-after(substring-after(substring-after(normalize-space(COL20),' '),' '),' '),' '),'#.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($ExpiryMonth)"/>
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
			<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>

			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>

		</xsl:if>
	</xsl:template>

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

	</xsl:template>

	<xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//Comparision">
        <xsl:if test="number(COL22)">
          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="'JPM'"/>
            </xsl:variable>

			  <xsl:variable name="PB_SYMBOL_NAME">
				  <xsl:value-of select="COL20"/>
			  </xsl:variable>
			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>

            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL3"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/AccountMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name = "PRANA_CounterParty">
              <xsl:value-of select="COL30"/>
            </xsl:variable>

            <xsl:variable name="PRANA_CounterPartyID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$varPBName]/BrokerData[@PranaBroker=$PRANA_CounterParty]/@PranaBrokerCode"/>
            </xsl:variable>

            <xsl:variable name="varCUSIP">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varRIC">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varBloomberg">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="COL19"/>
            </xsl:variable>


            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="COL19"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <xsl:variable name="varPositionSettlementDate">
              <xsl:value-of select="COL12"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="COL37"/>
            </xsl:variable>


            <xsl:variable name="varDescription">
              <xsl:value-of select="COL20"/>
            </xsl:variable>

            <xsl:variable name="varSide">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL21"/>
            </xsl:variable>

            <xsl:variable name="varCostBasis">
              <xsl:value-of select="COL23"/>
            </xsl:variable>

            <xsl:variable name="varFXConversionMethodOperator">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varFXRate">
              <xsl:value-of select="''"/>
            </xsl:variable>


            <xsl:variable name ="varStampDuty">
              <xsl:choose>
                <xsl:when test ="COL2 = 'USD' and COL17 ='EQTY' and (COL28 = 'SELL' or COL28 = 'SELL SHORT')">
                  <xsl:value-of select ="COL22*COL23*0.0000174"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
          

                <AccountName>
                  <xsl:value-of select="'Crestline account'"/>
                </AccountName>

			  <xsl:variable name="varAsset">
				  <xsl:choose>
					  <xsl:when test="substring-before(COL20,' ') ='CALL' or substring-before(COL20,' ') ='PUT'">
						  <xsl:value-of select="'EquityOption'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'Equity'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="Symbol">
				  <xsl:value-of select="COL19"/>
			  </xsl:variable>
			  <Symbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>

					  <xsl:when test="$varAsset='EquityOption'">
						  <xsl:call-template name="Option">
							  <xsl:with-param name="Symbol" select="COL8"/>
							  <xsl:with-param name="Suffix" select="''"/>
						  </xsl:call-template>
					  </xsl:when>

					  <xsl:when test="$Symbol!=''">
						  <xsl:value-of select="$Symbol"/>
					  </xsl:when>

					  <xsl:otherwise>

						  <xsl:value-of select="$PB_SYMBOL_NAME"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbol>

            <AvgPx>
              <xsl:choose>
                <xsl:when test ="boolean(number($varCostBasis))">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AvgPx>


            <!--QUANTITY-->

            <xsl:choose>
              <xsl:when test="$varNetPosition &lt; 0">
                <Quantity>
                  <xsl:value-of select="$varNetPosition * (-1)"/>
                </Quantity>
              </xsl:when>
              <xsl:when test="$varNetPosition &gt; 0">
                <Quantity>
                  <xsl:value-of select="$varNetPosition"/>
                </Quantity>
              </xsl:when>
              <xsl:otherwise>
                <Quantity>
                  <xsl:value-of select="0"/>
                </Quantity>
              </xsl:otherwise>
            </xsl:choose>
            
            <Side>
				<xsl:choose>
					<xsl:when test="$varAsset='EquityOption'">
						<xsl:choose>
							<xsl:when test="$varSide = 'BUY' or $varSide = 'CLOSE CONTRACT'">
								<xsl:value-of select="'Buy to Open'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'SELL' or $varSide = 'WRITE CONTRACT'">
								<xsl:value-of select="'Sell to Close'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'COVER SHORT'">
								<xsl:value-of select="'Buy to Close'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'SELL SHORT' ">
								<xsl:value-of select="'Sell to Open'"/>
							</xsl:when>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$varSide = 'BUY' or $varSide = 'CLOSE CONTRACT'">
								<xsl:value-of select="'Buy'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'SELL' or $varSide = 'WRITE CONTRACT'">
								<xsl:value-of select="'Sell'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'COVER SHORT'">
								<xsl:value-of select="'Buy to Close'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'SELL SHORT' ">
								<xsl:value-of select="'Sell short'"/>
							</xsl:when>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
				
             
            </Side>



			  <xsl:variable name="NetNotionalValue">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL22"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <NetNotionalValue>
				  <xsl:choose>
					  <xsl:when test="$NetNotionalValue &gt; 0">
						  <xsl:value-of select="$NetNotionalValue"/>
					  </xsl:when>
					  <xsl:when test="$NetNotionalValue &lt; 0">
						  <xsl:value-of select="$NetNotionalValue*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetNotionalValue>

			  <NetNotionalValueBase>
				  <xsl:choose>
					  <xsl:when test="$NetNotionalValue &gt; 0">
						  <xsl:value-of select="$NetNotionalValue"/>
					  </xsl:when>
					  <xsl:when test="$NetNotionalValue &lt; 0">
						  <xsl:value-of select="$NetNotionalValue*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetNotionalValueBase>

			  <xsl:variable name="varCommission">
				  <xsl:value-of select="''"/>
			  </xsl:variable>

			  <xsl:variable name="varFees">
				  <xsl:value-of select="''"/>
			  </xsl:variable>

			  <xsl:variable name="varMiscFee">
				  <xsl:value-of select="''"/>
			  </xsl:variable>

			 
			  <Commission>
				  <xsl:choose>
					  <xsl:when test="$varCommission &gt; 0">
						  <xsl:value-of select="$varCommission"/>
					  </xsl:when>
					  <xsl:when test="$varCommission &lt; 0">
						  <xsl:value-of select="$varCommission*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <MiscFees>
              <xsl:choose>
                <xsl:when test="$varMiscFee &gt; 0">
                  <xsl:value-of select="$varMiscFee"/>
                </xsl:when>
                <xsl:when test="$varMiscFee &lt; 0">
                  <xsl:value-of select="$varMiscFee*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MiscFees>

            <Fees>
              <xsl:choose>
                <xsl:when test="$varFees &gt; 0">
                  <xsl:value-of select="$varFees"/>
                </xsl:when>
                <xsl:when test="$varFees &lt; 0">
                  <xsl:value-of select="$varFees*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>

			  <TradeDate>


				  <xsl:value-of select="COL11"/>
			  </TradeDate>

			  <xsl:variable name ="Date1" select="COL4"/>


			  <xsl:variable name="Year" select="substring-after(substring-after($Date1,'/'),'/')"/>
			  <xsl:variable name="Month1" select="substring-before(substring-after($Date1,'/'),'/')"/>
			  <xsl:variable name="Day1" select="substring-before($Date1,'/')"/>


			  <SettlementDate>
				  <xsl:value-of select="COL12"/>
				  <!--<xsl:value-of select="concat($Month1,'/',$Day1,'/',$Year)"/>-->
			  </SettlementDate>

			  <StampDuty>
              <xsl:choose>
                <xsl:when test="$varStampDuty &gt; 0">
                  <xsl:value-of select="$varStampDuty"/>
                </xsl:when>
                <xsl:when test="$varStampDuty &lt; 0">
                  <xsl:value-of select="$varStampDuty*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </StampDuty>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
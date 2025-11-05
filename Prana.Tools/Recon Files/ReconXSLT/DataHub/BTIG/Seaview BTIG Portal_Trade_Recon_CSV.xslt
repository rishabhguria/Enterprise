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

	  <xsl:template name="MonthCode">
		  <xsl:param name="Month"/>
		  <xsl:param name="PutOrCall"/>
		  <xsl:if test="$PutOrCall='C'">
			  <xsl:choose>
				  <xsl:when test="$Month=01 ">
					  <xsl:value-of select="'A'"/>
				  </xsl:when>
				  <xsl:when test="$Month=02">
					  <xsl:value-of select="'B'"/>
				  </xsl:when>
				  <xsl:when test="$Month=03">
					  <xsl:value-of select="'C'"/>
				  </xsl:when>
				  <xsl:when test="$Month=04">
					  <xsl:value-of select="'D'"/>
				  </xsl:when>
				  <xsl:when test="$Month=05">
					  <xsl:value-of select="'E'"/>
				  </xsl:when>
				  <xsl:when test="$Month=06">
					  <xsl:value-of select="'F'"/>
				  </xsl:when>
				  <xsl:when test="$Month=07">
					  <xsl:value-of select="'G'"/>
				  </xsl:when>
				  <xsl:when test="$Month=08">
					  <xsl:value-of select="'H'"/>
				  </xsl:when>
				  <xsl:when test="$Month=09">
					  <xsl:value-of select="'I'"/>
				  </xsl:when>
				  <xsl:when test="$Month=10">
					  <xsl:value-of select="'J'"/>
				  </xsl:when>
				  <xsl:when test="$Month=11">
					  <xsl:value-of select="'K'"/>
				  </xsl:when>
				  <xsl:when test="$Month=12">
					  <xsl:value-of select="'L'"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="''"/>
				  </xsl:otherwise>
			  </xsl:choose>
		  </xsl:if>
		  <xsl:if test="$PutOrCall='P'">
			  <xsl:choose>
				  <xsl:when test="$Month=01">
					  <xsl:value-of select="'M'"/>
				  </xsl:when>
				  <xsl:when test="$Month=02">
					  <xsl:value-of select="'N'"/>
				  </xsl:when>
				  <xsl:when test="$Month=03">
					  <xsl:value-of select="'O'"/>
				  </xsl:when>
				  <xsl:when test="$Month=04">
					  <xsl:value-of select="'P'"/>
				  </xsl:when>
				  <xsl:when test="$Month=05">
					  <xsl:value-of select="'Q'"/>
				  </xsl:when>
				  <xsl:when test="$Month=06">
					  <xsl:value-of select="'R'"/>
				  </xsl:when>
				  <xsl:when test="$Month=07">
					  <xsl:value-of select="'S'"/>
				  </xsl:when>
				  <xsl:when test="$Month=08">
					  <xsl:value-of select="'T'"/>
				  </xsl:when>
				  <xsl:when test="$Month=09">
					  <xsl:value-of select="'U'"/>
				  </xsl:when>
				  <xsl:when test="$Month=10">
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
		  <xsl:if test="contains(substring(substring-after(normalize-space(COL6),' '),7,1),'C') or contains(substring(substring-after(normalize-space(COL6),' '),7,1),'P')">
			  <xsl:variable name="UnderlyingSymbol">
				  <xsl:value-of select="substring-before(COL6,' ')"/>
			  </xsl:variable>
			  <xsl:variable name="ExpiryDay">
				  <xsl:value-of select="substring(substring-after(normalize-space(COL6),' '),5,2)"/>
			  </xsl:variable>
			  <xsl:variable name="ExpiryMonth">
				  <xsl:value-of select="substring(substring-after(normalize-space(COL6),' '),3,2)"/>
			  </xsl:variable>
			  <xsl:variable name="ExpiryYear">
				  <xsl:value-of select="substring(substring-after(normalize-space(COL6),' '),1,2)"/>
			  </xsl:variable>

			  <xsl:variable name="PutORCall">
				  <xsl:value-of select="substring(substring-after(normalize-space(COL6),' '),7,1)"/>
			  </xsl:variable>
			  <xsl:variable name="StrikePrice">
				  <xsl:value-of select="format-number(substring(substring-after(normalize-space(COL6),' '),8) div 1000 ,'#.00')"/>
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

			  <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>

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

	  <!--<xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="COL18='Option'">

      -->
	  <!--</xsl:otherwise>-->
	  <!--
      -->
	  <!--

			</xsl:choose>-->
	  <!--
    </xsl:if>-->
  <!--
  </xsl:template>-->

  <xsl:template match="/">


    <DocumentElement>

      <xsl:for-each select ="//Comparision">


        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL5"/>
          </xsl:call-template>
        </xsl:variable>
		  <xsl:choose>
        <xsl:when test="number($Quantity)and not(contains(COL6, '380992966'))">

          <PositionMaster>
            <xsl:variable name ="varPBName">
              <xsl:value-of select ="'BTIG'"/>
            </xsl:variable>
            <xsl:variable name ="PB_FUND_NAME">
              <xsl:value-of select ="COL26"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <FundName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </FundName>

			  <xsl:variable name="Asset">
				  <xsl:choose>
					  <xsl:when test="string-length(COL6) &gt; 10">
						  <xsl:value-of select="'Option'"/>
					  </xsl:when>
				  </xsl:choose>

			  </xsl:variable>



			  <xsl:variable name ="Symbol">
              <xsl:value-of select ="COL6"/>
            </xsl:variable>
            <xsl:variable name ="PB_COMPANY">

              <xsl:value-of select ="COL7"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL">

              <xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
				<xsl:choose>
					<xsl:when test="$PRANA_SYMBOL!=''">
						<xsl:value-of select="$PRANA_SYMBOL"/>
					</xsl:when>

					<xsl:when test="$Asset='Option'">
						<xsl:call-template name="Option">
						<xsl:with-param name="Symbol" select="COL6"/>
						<xsl:with-param name="Suffix" select="''"/>
						</xsl:call-template>
					</xsl:when>

					<xsl:when test="string-length(COL6)=7 or string-length(COL6) &gt; 7">
						<xsl:value-of select="''"/>
					</xsl:when>

					<xsl:when test="COL6!='*'">
						<xsl:value-of select="COL6"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$PB_COMPANY"/>
					</xsl:otherwise>
				</xsl:choose>
            </Symbol>

			  <!--<IDCOOptionSymbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="$Asset='Option'">
						  <xsl:value-of select="concat(COL6,'U')"/>
					  </xsl:when>

					  <xsl:when test="string-length(COL6)=7 or string-length(COL6) &gt; 7">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="COL6!='*'">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </IDCOOptionSymbol>-->

			  <SEDOL>

				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="$Asset='Option'">
						  <xsl:value-of select="''"/>
					  </xsl:when>


					  <xsl:when test="string-length(COL6)=7 or string-length(COL6) &gt; 7">
						  <xsl:value-of select="COL6"/>
					  </xsl:when>
					  <xsl:when test="COL6!='*'">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>

			  </SEDOL>





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





            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL9"/>
              </xsl:call-template>
            </xsl:variable>


            <AvgPX>
              <xsl:choose>
                <xsl:when test="$AvgPrice &gt; 0">
                  <xsl:value-of select="$AvgPrice"/>

                </xsl:when>
                <xsl:when test="$AvgPrice &lt; 0">
                  <xsl:value-of select="$AvgPrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </AvgPX>



            <xsl:variable name="Side" select="COL4"/>

            <Side>
				<xsl:choose>
					<xsl:when test="$Asset='Option'">
						<xsl:choose>
							<xsl:when test="COL4='BUY'">
								<xsl:value-of select="'Buy to Open'"/>
							</xsl:when>

							<xsl:when test="COL4='SEL'">
								<xsl:value-of select="'Sell to Close'"/>
							</xsl:when>
							<xsl:when test="COL4='SSL'">
								<xsl:value-of select="'Sell to Open'"/>
							</xsl:when>
							<xsl:when test="COL4='BTC'">
								<xsl:value-of select="'Buy to Close'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>

					<xsl:otherwise>

						<xsl:choose>
							<xsl:when test="COL4='BUY'">
								<xsl:value-of select="'Buy'"/>
							</xsl:when>

							<xsl:when test="COL4='SEL'">
								<xsl:value-of select="'Sell'"/>
							</xsl:when>
							<xsl:when test="COL4='SSL'">
								<xsl:value-of select="'Sell Short'"/>
							</xsl:when>
							<xsl:when test="COL4='BTC'">
								<xsl:value-of select="'Buy to Close'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>



				</xsl:choose>
            </Side>






            <CompanyName>
              <xsl:value-of select="$PB_COMPANY"/>
            </CompanyName>



            <xsl:variable name ="Month">
				<xsl:choose>
					<xsl:when test="string-length(substring-before(COL2,'/')) ='1'">
						<xsl:value-of select ="concat('0',substring-before(COL2,'/'))"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="substring-before(COL2,'/')"/>
					</xsl:otherwise>
				</xsl:choose>		
        
            </xsl:variable>
			  
            <xsl:variable name ="Date">
				<xsl:choose>
					<xsl:when test="string-length(substring-before(substring-after(COL2,'/'),'/')) ='1'">
						<xsl:value-of select ="concat('0',substring-before(substring-after(COL2,'/'),'/'))"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="substring-before(substring-after(COL2,'/'),'/')"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
            <xsl:variable name ="Year">
				<xsl:value-of select ="substring-after(substring-after(COL2,'/'),'/')"/>
            </xsl:variable>

            <TradeDate>
              <xsl:value-of select ="concat($Month,'/',$Date,'/',$Year)"/>

            </TradeDate>


			  <xsl:variable name ="varMonth">
				  <xsl:choose>
					  <xsl:when test="string-length(substring-before(COL3,'/')) ='1'">
						  <xsl:value-of select ="concat('0',substring-before(COL3,'/'))"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="substring-before(COL3,'/')"/>
					  </xsl:otherwise>
				  </xsl:choose>

			  </xsl:variable>

			  <xsl:variable name ="varDate">
				  <xsl:choose>
					  <xsl:when test="string-length(substring-before(substring-after(COL3,'/'),'/')) ='1'">
						  <xsl:value-of select ="concat('0',substring-before(substring-after(COL3,'/'),'/'))"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="substring-before(substring-after(COL3,'/'),'/')"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  <xsl:variable name ="varYear">
				  <xsl:value-of select ="substring-after(substring-after(COL3,'/'),'/')"/>
			  </xsl:variable>

			  <SettlementDate>
				  <xsl:value-of select ="concat($varMonth,'/',$varDate,'/',$varYear)"/>
			  </SettlementDate>



            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="translate(COL18,',','')"/>
              </xsl:call-template>
            </xsl:variable>

            <Commission>
              <xsl:choose>
                <xsl:when test =" $Commission &gt; 0">
                  <xsl:value-of select ="$Commission"/>
                </xsl:when>
                <xsl:when test ="$Commission &lt; 0">
                  <xsl:value-of select ="$Commission * -1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

			  <OtherBrokerFees>
				  <xsl:choose>
					  <xsl:when test="contains(COL4,'BUY') or contains(COL4,'BTC')">
						  <xsl:value-of select="COL13"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'0'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </OtherBrokerFees>

            <SecFee>
              <xsl:choose>

                <xsl:when test="COL4='SEL' or COL4='SSL'">
                  <xsl:value-of select="COL13"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'0'"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>


			  <xsl:variable name="varCOL18">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="translate(COL18,',','')"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <xsl:variable name="varCOL19">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="translate(COL19,',','')"/>
				  </xsl:call-template>
			  </xsl:variable>



			  

			  <!--<xsl:variable name="TotalCommissionandFees">
				  --><!--<xsl:choose>
					  <xsl:when test="COL19 =' ' or COL19 ='*'">
						  <xsl:value-of select="$varCOL18"/>
					  </xsl:when>
					  <xsl:otherwise>--><!--
						  <xsl:value-of select="$varCOL18 + $varCOL19"/>
					  --><!--</xsl:otherwise>

				  </xsl:choose>--><!--
			  </xsl:variable>-->
			  <xsl:variable name="Commission1">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="translate(COL19,',','')"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <xsl:variable name="VarCommission1">
				  <xsl:choose>
					  <xsl:when test="$Commission1 &gt; 0">
						  <xsl:value-of select="$Commission1"/>
					  </xsl:when>
					  <xsl:when test="$Commission1 &lt; 0">
						  <xsl:value-of select="$Commission1* -1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="VarCommission">
				  <xsl:choose>
					  <xsl:when test="$Commission &gt; 0">
						  <xsl:value-of select="$Commission"/>
					  </xsl:when>
					  <xsl:when test="$Commission &lt; 0">
						  <xsl:value-of select="$Commission* -1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  
			  
			  

			  <xsl:variable name="TotalCommissionandFees" select="(COL12 + COL13 + COL19)"/>
			  
			  <TotalCommissionandFeesBase>
				  <xsl:choose>
					  <xsl:when test="$TotalCommissionandFees &gt; 0">
						  <xsl:value-of select="$TotalCommissionandFees"/>
					  </xsl:when>
					  <xsl:when test="$TotalCommissionandFees &lt; 0">
						  <xsl:value-of select="$TotalCommissionandFees*-1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </TotalCommissionandFeesBase>


			  <xsl:variable name="GrossNotionalValue">
				  <xsl:choose>
					  <xsl:when test="$Asset='Option'">
						  <xsl:value-of select="format-number(translate(COL5,',','') * translate(COL9,',','') * 100, '#.##')"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="format-number(translate(COL5,',','') * translate(COL9,',',''), '#.##')"/>
					  </xsl:otherwise>
				  </xsl:choose>
				 
			  </xsl:variable>



			  <GrossNotionalValue>
				  <xsl:choose>
					  <xsl:when test="$GrossNotionalValue &gt; 0">
						  <xsl:value-of select="$GrossNotionalValue"/>
					  </xsl:when>
					  <xsl:when test="$GrossNotionalValue &lt; 0">
						  <xsl:value-of select="$GrossNotionalValue*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </GrossNotionalValue>

            <xsl:variable name="NetNotional">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL15"/>
              </xsl:call-template>
            </xsl:variable>


			  <NetNotionalValue>
              <xsl:choose>
                <xsl:when test =" $NetNotional &gt; 0">
                  <xsl:value-of select ="$NetNotional"/>
                </xsl:when>
                <xsl:when test ="$NetNotional &lt; 0">
                  <xsl:value-of select ="$NetNotional * -1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

            <xsl:variable name="NetNotional1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL22"/>
              </xsl:call-template>
            </xsl:variable>


			  <NetNotionalValueBase>
				  <xsl:choose>
					  <xsl:when test =" $NetNotional1 &gt; 0">
						  <xsl:value-of select ="$NetNotional1"/>
					  </xsl:when>
					  <xsl:when test ="$NetNotional1 &lt; 0">
						  <xsl:value-of select ="$NetNotional1 * -1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetNotionalValueBase>

            <BaseCurrency>
              <xsl:value-of select="COL8"/>
            </BaseCurrency>
			  <TotalCommissionandFees>
				  <xsl:value-of select="$TotalCommissionandFees"/>
			  </TotalCommissionandFees>

            <SettlCurrency>
              <xsl:value-of select="COL10"/>
            </SettlCurrency>

            <SMRequest>
              <xsl:value-of select="'true'"/>
            </SMRequest>

			  <PrimeBroker>
				  <xsl:value-of select="COL24"/>
			  </PrimeBroker>

          </PositionMaster>

		</xsl:when>
		<xsl:otherwise>





			<PositionMaster>
				<xsl:variable name="PB_NAME">
					<xsl:value-of select="'BTIG'"/>
				</xsl:variable>

				<xsl:variable name="PB_SYMBOL_NAME">
					<xsl:value-of select="COL7"/>
				</xsl:variable>

				<xsl:variable name="PRANA_SYMBOL_NAME">
					<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
				</xsl:variable>


				<xsl:variable name = "PB_SUFFIX_CODE" >
					<xsl:value-of select ="substring-after(COL10,' ')"/>
				</xsl:variable>

				<xsl:variable name ="PRANA_SUFFIX_NAME">
					<xsl:value-of select="document('../../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
				</xsl:variable>

				<xsl:variable name="varSymbol">
					<xsl:choose>
						<xsl:when test="contains(COL10,' ')">
							<xsl:value-of select="substring-before(COL10,' ')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="COL10"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>



				<Symbol>
					<xsl:choose>
						<xsl:when test="$PRANA_SYMBOL_NAME!=''">
							<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
						</xsl:when>

						<xsl:when test="$varSymbol!=''">
							<xsl:value-of select="$varSymbol"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</xsl:otherwise>

					</xsl:choose>
				</Symbol>


				<xsl:variable name="PB_FUND_NAME" select="''"/>
				<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>
				<FundName>
					<xsl:choose>
						<xsl:when test ="$PRANA_FUND_NAME!=''">
							<xsl:value-of select ="$PRANA_FUND_NAME"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="$PB_FUND_NAME"/>
						</xsl:otherwise>
					</xsl:choose>
				</FundName>

				<Quantity>
					<xsl:choose>
						<xsl:when test="$Quantity &gt; 0">
							<xsl:value-of select="$Quantity"/>
						</xsl:when>

						<xsl:when test="$Quantity &lt; 0">
							<xsl:value-of select="$Quantity * -1"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</Quantity>

				<xsl:variable name="AvgPrice">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL13"/>
					</xsl:call-template>
				</xsl:variable>
				<AvgPX>
					<xsl:choose>
						<xsl:when test="$AvgPrice &gt; 0">
							<xsl:value-of select="$AvgPrice"/>

						</xsl:when>
						<xsl:when test="$AvgPrice &lt; 0">
							<xsl:value-of select="$AvgPrice * (-1)"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>

					</xsl:choose>
				</AvgPX>



				<xsl:variable name="Commission">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL16"/>
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
				</Commission>

				<xsl:variable name="FxRate">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL19"/>
					</xsl:call-template>
				</xsl:variable>
				<FxRate>
					<xsl:choose>
						<xsl:when test="number($FxRate)">
							<xsl:value-of select="$FxRate"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="1"/>
						</xsl:otherwise>
					</xsl:choose>
				</FxRate>

				<xsl:variable name="varSecFee">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<SecFee>
					<xsl:choose>
						<xsl:when test="$varSecFee &gt; 0">
							<xsl:value-of select="$varSecFee"/>
						</xsl:when>
						<xsl:when test="$varSecFee &lt; 0">
							<xsl:value-of select="$varSecFee * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</SecFee>
				<xsl:variable name="varFee">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL17"/>
					</xsl:call-template>
				</xsl:variable>
				<Fees>
					<xsl:choose>
						<xsl:when test="$varFee &gt; 0">
							<xsl:value-of select="$varFee"/>
						</xsl:when>
						<xsl:when test="$varFee &lt; 0">
							<xsl:value-of select="$varFee * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</Fees>

				<xsl:variable name="ClearingFee">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<ClearingFee>
					<xsl:choose>
						<xsl:when test="$ClearingFee &gt; 0">
							<xsl:value-of select="$ClearingFee"/>
						</xsl:when>
						<xsl:when test="$ClearingFee &lt; 0">
							<xsl:value-of select="$ClearingFee * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</ClearingFee>


				<xsl:variable name="AUECFee1">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<AUECFee1>
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
				</AUECFee1>

				<xsl:variable name="AUECFee2">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<AUECFee2>
					<xsl:choose>
						<xsl:when test="$AUECFee2 &gt; 0">
							<xsl:value-of select="$AUECFee2"/>
						</xsl:when>
						<xsl:when test="$AUECFee2 &lt; 0">
							<xsl:value-of select="$AUECFee2 * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</AUECFee2>

				<xsl:variable name="StampDuty">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<StampDuty>
					<xsl:choose>
						<xsl:when test="$StampDuty &gt; 0">
							<xsl:value-of select="$StampDuty"/>
						</xsl:when>
						<xsl:when test="$StampDuty &lt; 0">
							<xsl:value-of select="$StampDuty * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</StampDuty>




				<xsl:variable name="GrossNotionalValue">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<GrossNotionalValue>
					<xsl:choose>
						<xsl:when test="$GrossNotionalValue &gt; 0">
							<xsl:value-of select="$GrossNotionalValue"/>
						</xsl:when>
						<xsl:when test="$GrossNotionalValue &lt; 0">
							<xsl:value-of select="$GrossNotionalValue * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</GrossNotionalValue>

				<xsl:variable name="GrossNotionalValueBase">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<GrossNotionalValueBase>
					<xsl:choose>
						<xsl:when test="$GrossNotionalValueBase &gt; 0">
							<xsl:value-of select="$GrossNotionalValueBase"/>
						</xsl:when>
						<xsl:when test="$GrossNotionalValueBase &lt; 0">
							<xsl:value-of select="$GrossNotionalValueBase * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</GrossNotionalValueBase>



				<xsl:variable name="NetNaotionalValue">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL14"/>
					</xsl:call-template>
				</xsl:variable>
				<NetNotionalValue>
					<xsl:choose>
						<xsl:when test="$NetNaotionalValue &gt; 0">
							<xsl:value-of select="$NetNaotionalValue"/>
						</xsl:when>
						<xsl:when test="$NetNaotionalValue &lt; 0">
							<xsl:value-of select="$NetNaotionalValue * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</NetNotionalValue>

				<xsl:variable name="NetNaotionalValueBase">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL20"/>
					</xsl:call-template>
				</xsl:variable>
				<NetNotionalValueBase>
					<xsl:choose>
						<xsl:when test="$NetNaotionalValueBase &gt; 0">
							<xsl:value-of select="$NetNaotionalValueBase"/>
						</xsl:when>
						<xsl:when test="$NetNaotionalValueBase &lt; 0">
							<xsl:value-of select="$NetNaotionalValueBase * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</NetNotionalValueBase>


				<xsl:variable name="TotalCommissionandFees">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL16 + COL17"/>
					</xsl:call-template>
				</xsl:variable>

				<TotalCommissionandFees>
					<xsl:choose>
						<xsl:when test="$TotalCommissionandFees &gt; 0">
							<xsl:value-of select="$TotalCommissionandFees"/>
						</xsl:when>
						<xsl:when test="$TotalCommissionandFees &lt; 0">
							<xsl:value-of select="$TotalCommissionandFees * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</TotalCommissionandFees>

				<xsl:variable name="TotalCommissionandFeesBase">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL22 + COL23"/>
					</xsl:call-template>
				</xsl:variable>

				<TotalCommissionandFeesBase>
					<xsl:choose>
						<xsl:when test="$TotalCommissionandFeesBase &gt; 0">
							<xsl:value-of select="$TotalCommissionandFeesBase"/>
						</xsl:when>
						<xsl:when test="$TotalCommissionandFeesBase &lt; 0">
							<xsl:value-of select="$TotalCommissionandFeesBase * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</TotalCommissionandFeesBase>


				<xsl:variable name="ClearingBrokerFeeBase">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<ClearingBrokerFeeBase>
					<xsl:choose>
						<xsl:when test="$ClearingBrokerFeeBase &gt; 0">
							<xsl:value-of select="$ClearingBrokerFeeBase"/>
						</xsl:when>
						<xsl:when test="$ClearingBrokerFeeBase &lt; 0">
							<xsl:value-of select="$ClearingBrokerFeeBase * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</ClearingBrokerFeeBase>


				<xsl:variable name="SoftCommission">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<SoftCommission>
					<xsl:choose>
						<xsl:when test="$SoftCommission &gt; 0">
							<xsl:value-of select="$SoftCommission"/>
						</xsl:when>
						<xsl:when test="$SoftCommission &lt; 0">
							<xsl:value-of select="$SoftCommission * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</SoftCommission>




				<xsl:variable name="TaxOnCommissions">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<TaxOnCommissions>
					<xsl:choose>
						<xsl:when test="$TaxOnCommissions &gt; 0">
							<xsl:value-of select="$TaxOnCommissions"/>
						</xsl:when>
						<xsl:when test="$TaxOnCommissions &lt; 0">
							<xsl:value-of select="$TaxOnCommissions * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>

				</TaxOnCommissions>



				<xsl:variable name="UnitCost">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<UnitCost>
					<xsl:choose>
						<xsl:when test="$UnitCost &gt; 0">
							<xsl:value-of select="$UnitCost"/>
						</xsl:when>
						<xsl:when test="$UnitCost &lt; 0">
							<xsl:value-of select="$UnitCost * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>

				</UnitCost>




				<BaseCurrency>
					<xsl:value-of select="''"/>
				</BaseCurrency>


				<SettlCurrency>
					<xsl:value-of select="''"/>
				</SettlCurrency>


				<xsl:variable name="SettlCurrFxRate">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<SettlCurrFxRate>
					<xsl:choose>
						<xsl:when test="number($SettlCurrFxRate)">
							<xsl:value-of select="$SettlCurrFxRate"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>

				</SettlCurrFxRate>


				<xsl:variable name="SettlCurrAmt">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<SettlCurrAmt>

					<xsl:choose>
						<xsl:when test="number($SettlCurrAmt)">
							<xsl:value-of select="$SettlCurrAmt"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</SettlCurrAmt>


				<xsl:variable name="SettlPrice">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<SettlPrice>

					<xsl:choose>
						<xsl:when test="number($SettlPrice)">
							<xsl:value-of select="$SettlPrice"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</SettlPrice>

				<xsl:variable name="MiscFees">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="''"/>
					</xsl:call-template>
				</xsl:variable>
				<MiscFees>
					<xsl:choose>
						<xsl:when test="$MiscFees &gt; 0">
							<xsl:value-of select="$MiscFees"/>
						</xsl:when>
						<xsl:when test="$MiscFees &lt; 0">
							<xsl:value-of select="$MiscFees * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</MiscFees>


				<xsl:variable name ="Month">
					<xsl:choose>
						<xsl:when test="string-length(substring-before(COL3,'/')) ='1'">
							<xsl:value-of select ="concat('0',substring-before(COL3,'/'))"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="substring-before(COL3,'/')"/>
						</xsl:otherwise>
					</xsl:choose>

				</xsl:variable>

				<xsl:variable name ="Date">
					<xsl:choose>
						<xsl:when test="string-length(substring-before(substring-after(COL3,'/'),'/')) ='1'">
							<xsl:value-of select ="concat('0',substring-before(substring-after(COL3,'/'),'/'))"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="substring-before(substring-after(COL3,'/'),'/')"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name ="Year">
					<xsl:value-of select ="substring-after(substring-after(COL3,'/'),'/')"/>
				</xsl:variable>

				<TradeDate>
					<xsl:value-of select ="concat($Month,'/',$Date,'/',$Year)"/>

				</TradeDate>


				<xsl:variable name ="varMonth">
					<xsl:choose>
						<xsl:when test="string-length(substring-before(COL4,'/')) ='1'">
							<xsl:value-of select ="concat('0',substring-before(COL4,'/'))"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="substring-before(COL4,'/')"/>
						</xsl:otherwise>
					</xsl:choose>

				</xsl:variable>

				<xsl:variable name ="varDate">
					<xsl:choose>
						<xsl:when test="string-length(substring-before(substring-after(COL4,'/'),'/')) ='1'">
							<xsl:value-of select ="concat('0',substring-before(substring-after(COL4,'/'),'/'))"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="substring-before(substring-after(COL4,'/'),'/')"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name ="varYear">
					<xsl:value-of select ="substring-after(substring-after(COL4,'/'),'/')"/>
				</xsl:variable>

				<SettlementDate>
					<xsl:value-of select ="concat($varMonth,'/',$varDate,'/',$varYear)"/>
				</SettlementDate>



				<CurrencySymbol>
					<xsl:value-of select="''"/>
				</CurrencySymbol>



				<xsl:variable name="varSide">
					<xsl:value-of select="COL60"/>
				</xsl:variable>
				<Side>
					<xsl:choose>
						<xsl:when test="$Quantity &gt; 0">
							<xsl:value-of select="'Buy'"/>
						</xsl:when>
						<xsl:when test="$Quantity &lt; 0">
							<xsl:value-of select="'Sell'"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>

					</xsl:choose>
				</Side>


				<PBSymbol>
					<xsl:value-of select="$PB_SYMBOL_NAME"/>
				</PBSymbol>


				<SMRequest>
					<xsl:value-of select="'true'"/>
				</SMRequest>

			</PositionMaster>
		</xsl:otherwise>
	</xsl:choose>
</xsl:for-each>
</DocumentElement>
</xsl:template>


<xsl:template name="FormatDate">
<xsl:param name="DateTime" />
<!-- converts date time double number to 18/12/2009 -->

<xsl:variable name="l">
<xsl:value-of select="$DateTime + 68569 + 2415019" />
</xsl:variable>

<xsl:variable name="n">
<xsl:value-of select="floor(((4 * $l) div 146097))" />
</xsl:variable>

<xsl:variable name="ll">
<xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
</xsl:variable>

<xsl:variable name="i">
<xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
</xsl:variable>

<xsl:variable name="lll">
<xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
</xsl:variable>

<xsl:variable name="j">
<xsl:value-of select="floor(((80 * $lll) div 2447))" />
</xsl:variable>

<xsl:variable name="nDay">
<xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
</xsl:variable>

<xsl:variable name="llll">
<xsl:value-of select="floor(($j div 11))" />
</xsl:variable>

<xsl:variable name="nMonth">
<xsl:value-of select="floor($j + 2 - (12 * $llll))" />
</xsl:variable>

<xsl:variable name="nYear">
<xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
</xsl:variable>

<xsl:variable name ="varMonthUpdated">
<xsl:choose>
	<xsl:when test ="string-length($nMonth) = 1">
		<xsl:value-of select ="concat('0',$nMonth)"/>
	</xsl:when>
	<xsl:otherwise>
		<xsl:value-of select ="$nMonth"/>
	</xsl:otherwise>
</xsl:choose>
</xsl:variable>

<xsl:variable name ="nDayUpdated">
<xsl:choose>
	<xsl:when test ="string-length($nDay) = 1">
		<xsl:value-of select ="concat('0',$nDay)"/>
	</xsl:when>
	<xsl:otherwise>
		<xsl:value-of select ="$nDay"/>
	</xsl:otherwise>
</xsl:choose>
</xsl:variable>

<xsl:value-of select="$varMonthUpdated"/>
<xsl:value-of select="'/'"/>
<xsl:value-of select="$nDayUpdated"/>
<xsl:value-of select="'/'"/>
<xsl:value-of select="$nYear"/>

</xsl:template>

<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>



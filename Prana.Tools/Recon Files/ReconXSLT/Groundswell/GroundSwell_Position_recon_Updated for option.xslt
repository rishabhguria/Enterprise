<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" indent="yes"/>

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

  <xsl:template name="MonthNo">
		<xsl:param name="varMonth"/>

		<xsl:choose>
			<xsl:when test ="$varMonth= 'A' or $varMonth= 'M'">
				<xsl:value-of select ="1"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'B' or $varMonth= 'N'">
				<xsl:value-of select ="2"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'C' or $varMonth= 'O'">
				<xsl:value-of select ="3"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'D' or $varMonth= 'P'">
				<xsl:value-of select ="4"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'E' or $varMonth= 'Q'">
				<xsl:value-of select ="5"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'F' or $varMonth= 'R'">
				<xsl:value-of select ="6"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'G' or $varMonth= 'S'">
				<xsl:value-of select ="7"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'H' or $varMonth= 'T'">
				<xsl:value-of select ="8"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'I' or $varMonth= 'U'">
				<xsl:value-of select ="9"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'J' or $varMonth= 'V'">
				<xsl:value-of select ="10"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'K' or $varMonth= 'W'">
				<xsl:value-of select ="11"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'L' or $varMonth= 'X'">
				<xsl:value-of select ="12"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="ConvertBBCodetoTicker">
		<xsl:param name="varBBCode"/>

		<xsl:variable name="varRoot">
			<xsl:value-of select="substring-before($varBBCode,'1')"/>
		</xsl:variable>

		<xsl:variable name="varUnderlyingLength">
			<xsl:value-of select="string-length($varRoot)"/>
		</xsl:variable>

		<xsl:variable name="varExYear">
			<xsl:value-of select="substring($varBBCode,($varUnderlyingLength +1),2)"/>
		</xsl:variable>

		<xsl:variable name="varStrike">
			<xsl:value-of select="format-number(substring($varBBCode,($varUnderlyingLength +6)), '#.00')"/>
		</xsl:variable>

		<xsl:variable name="varExDay">
			<xsl:value-of select="substring($varBBCode,($varUnderlyingLength +3),2)"/>
		</xsl:variable>

		<xsl:variable name="varExpiryDay">
			<xsl:choose>
				<xsl:when test="substring($varExDay,1,1)= '0'">
					<xsl:value-of select="substring($varExDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varExDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varMonthCode">
			<xsl:value-of select="substring(COL3,($varUnderlyingLength + 5),1)"/>
		</xsl:variable>

		<xsl:variable name="varExMonth">
			<xsl:call-template name="MonthNo">
				<xsl:with-param name="varMonth" select="$varMonthCode"/>
			</xsl:call-template>
		</xsl:variable>

		<!--<xsl:variable name='varThirdFriday'>
			<xsl:choose>
				<xsl:when test="number($varExYear) and number($varExMonth)">
					<xsl:value-of select='my:Now(number(concat("20",$varExYear)),number($varExMonth))'/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
				<xsl:value-of select="normalize-space(concat('O:', $varRoot, ' ', $varExYear,$varMonthCode,$varStrike))"/>
			</xsl:when>
			<xsl:otherwise>-->
				<xsl:value-of select="normalize-space(concat('O:', $varRoot, ' ', $varExYear,$varMonthCode,$varStrike,'D',$varExpiryDay))"/>
			<!--</xsl:otherwise>
		</xsl:choose>-->

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


		  <xsl:variable name="PB_FUND_NAME" select="COL1"/>

		  <xsl:variable name="PRANA_FUND_NAME">
			  <xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name='Wedbush']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
		  </xsl:variable>
		  
        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL4"/>
          </xsl:call-template>
        </xsl:variable>

		  <xsl:if test="normalize-space(COL2)!='Cusip' and $PRANA_FUND_NAME!=''">

          <PositionMaster>

			  <xsl:variable name ="varCallPut">
				  <xsl:choose>
					  <xsl:when test="substring-before(COL6, ' ')= 'CALL' or substring-before(COL6, ' ')= 'PUT'">
						  <xsl:value-of select="1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>


			  <xsl:variable name="varUnderlying">
				  <xsl:choose>
					  <xsl:when test="COL3 != '' and COL3 != '*' and $varCallPut = 1">
						  <xsl:value-of select="substring-before(COL3,'1')"/>
					  </xsl:when>
					  <xsl:when test="COL3 = '' or COL3 = '*'">
						  <xsl:value-of select="normalize-space(COL2)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="normalize-space(COL3)"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="varUnderlyingLength">
				  <xsl:value-of select="string-length($varUnderlying)"/>
			  </xsl:variable>

			  <xsl:variable name="varMonthCode">
				  <xsl:if test ="$varCallPut = 1">
					  <xsl:value-of select="substring(COL3,($varUnderlyingLength + 5),1)"/>
				  </xsl:if>
			  </xsl:variable>

			  <xsl:variable name="varDateNo">
				  <xsl:if test="$varCallPut = 1">
					  <xsl:value-of select="substring(COL3,($varUnderlyingLength + 3),2)"/>
				  </xsl:if>
			  </xsl:variable>

			  <xsl:variable name="varExpirationYear">
				  <xsl:if test="$varCallPut = 1">
					  <xsl:value-of select="substring(COL3,($varUnderlyingLength + 1),2)"/>
				  </xsl:if>
			  </xsl:variable>

			  <xsl:variable name="varStrikePriceString">
				  <xsl:value-of select="substring-after(substring-after(COL3,$varUnderlying),$varMonthCode)"/>
			  </xsl:variable>


			  <xsl:variable name ="varStrikePrice">
				  <xsl:if test="$varCallPut = 1 and number($varStrikePriceString)">
					  <xsl:value-of select="format-number($varStrikePriceString,'#.00')"/>
				  </xsl:if>
			  </xsl:variable>

			  <xsl:variable name="varMonthNo">
				  <xsl:call-template name="MonthNo">
					  <xsl:with-param name="varMonth" select="$varMonthCode"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <!--<xsl:variable name="varThirdFriday">
              <xsl:value-of select =" my:Now($varExpirationYear,$varMonthNo)"/>
            </xsl:variable>

            <xsl:variable name="varIsFlex">
              <xsl:choose>
                <xsl:when test="substring-before(substring-after($varThirdFriday,'/'),'/')=(($varDateNo)-1)">
                  <xsl:value-of select="0"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="1"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->
			  <xsl:variable name="Date">
				  <xsl:choose>
					  <xsl:when test="substring($varDateNo,1,1)='0'">
						  <xsl:value-of select="substring($varDateNo,2,1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$varDateNo"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>



			  <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Wedbush'"/>
            </xsl:variable>

            <xsl:variable name="apos">'</xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:choose>
                <xsl:when test="contains(COL3,$apos)">
                  <xsl:value-of select="COL6"/>
                </xsl:when>
                <xsl:when test="contains(COL6,'XXX')">
                  <xsl:value-of select="concat(substring-before(COL6,'X'),' ',' ',' ')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="COL6"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>
			  <xsl:variable name="PB_NAME1">
				  <xsl:value-of select="'WedbushGR'"/>
			  </xsl:variable>
			  <xsl:variable name="PB_CUSIP_SYMBOL" select="normalize-space(COL2)"/>

			  <xsl:variable name="PRANA_CUSIP_SYMBOL">
				  <xsl:value-of select="document('../ReconMappingXml/CusipMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME1]/SymbolData[@CusipSymbol=$PB_CUSIP_SYMBOL]/@PranaSymbol"/>
			  </xsl:variable>
			 
			  

            <xsl:variable name="PRANA_SYMBOL">

				
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME1]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
					
              
            </xsl:variable>

            <xsl:variable name="Apos">'</xsl:variable>

            <xsl:variable name="After_Apos">
              <xsl:value-of select="substring-after(COL3,$Apos)"/>
            </xsl:variable>

            <xsl:variable name="Prana_Symbol_Apos">
              <xsl:value-of select="document('../ReconMappingXml/AposSymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME1]/SymbolData[@AfterApos=$After_Apos]/@PostSymbol"/>
            </xsl:variable>

            <xsl:variable name="Symbol">
              <xsl:choose>
                <xsl:when test="contains(COL3,'/')">
                  <xsl:value-of select="translate(COL3,'/','.')"/>
                </xsl:when>
                <xsl:when test="contains(COL3,$Apos)">
                  <xsl:choose>
                    <xsl:when test="substring-after(COL3,$Apos)=''">
                      <xsl:value-of select="concat(substring-before(COL3,$Apos),'/P')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="concat(substring-before(COL3,$Apos),'/',$Prana_Symbol_Apos)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$varCallPut = 1">
                      <xsl:call-template name="ConvertBBCodetoTicker">
                        <xsl:with-param name="varBBCode" select="COL3"/>
                      </xsl:call-template>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$varUnderlying"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <Symbol>
              <xsl:choose>

				  <xsl:when test ="$PRANA_CUSIP_SYMBOL != ''">
					  <xsl:value-of select ="$PRANA_CUSIP_SYMBOL"/>
				  </xsl:when>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>
                <!--<xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test="$varCallPut = 1">
								  <xsl:call-template name="ConvertBBCodetoTicker">
									  <xsl:with-param name="varBBCode" select="COL3"/>
								  </xsl:call-template>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select="$varUnderlying"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:otherwise>-->
                <xsl:when test="$Symbol!='*' or $Symbol!=''">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>
				  
				  <xsl:otherwise>
					  <xsl:choose>
						  <xsl:when test="$varCallPut = 1">
							  <xsl:call-template name="ConvertBBCodetoTicker">
								  <xsl:with-param name="varBBCode" select="COL3"/>
							  </xsl:call-template>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="$varUnderlying"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </xsl:otherwise>
			  </xsl:choose>
				
				
				
				<!--<xsl:choose>
					<xsl:when test ="$PRANA_SYMBOL != ''">
						<xsl:value-of select ="$PRANA_SYMBOL"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$varCallPut = 1">
								<xsl:call-template name="ConvertBBCodetoTicker">
									<xsl:with-param name="varBBCode" select="COL3"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$varUnderlying"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>-->
            </Symbol>

			  <!--<CUSIP>

				  <xsl:choose>

					  <xsl:when test ="$PRANA_CUSIP_SYMBOL != ''">
						  <xsl:value-of select ="''"/>
					  </xsl:when>
					  <xsl:when test ="$PRANA_SYMBOL != ''">
						  <xsl:value-of select ="''"/>
					  </xsl:when>
					 
					  <xsl:when test="$Symbol!='*' or $Symbol!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test="$varCallPut = 1">
								  <xsl:value-of select="''"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select="''"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:otherwise>
				  </xsl:choose>
				  
			  </CUSIP>-->


            <!--<IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>


                <xsl:when test="$Assettype='Option'">
                  <xsl:value-of select="concat($Symbol,'U')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </IDCOOptionSymbol>-->


            


            <!--<xsl:variable name="Year1" select="substring($Date,1,4)"/>
            <xsl:variable name="Month" select="substring($Date,5,2)"/>
            <xsl:variable name="Day" select="substring($Date,7,2)"/>-->



			  <PositionStartDate>
				  <xsl:value-of select="COL10"/>
			  </PositionStartDate>




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
                <xsl:when test="number($Quantity)">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>





            <xsl:variable name="Side" select="COL6"/>

            <Side>
              <xsl:choose>
                <xsl:when test="contains(COL6,'CALL') or contains(COL6,'PUT')">
                  <xsl:choose>
                    <xsl:when test="$Quantity &gt; 0">
                      <xsl:value-of select="'Buy to Open'"/>

                    </xsl:when>
                    <xsl:when test="$Quantity &lt; 0">
                      <xsl:value-of select="'Sell to Open'"/>
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
                  <xsl:value-of select="'Sell'"/>
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
                <xsl:with-param name="Number" select="COL4"/>
              </xsl:call-template>
            </xsl:variable>


            <MarkPrice>
              <xsl:choose>
                <xsl:when  test="number(COL5)">
                  <xsl:value-of select="COL5"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </MarkPrice>

            <!--<PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>-->




            <!--<xsl:variable name="MarketValueLocal">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL26"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValue>
              <xsl:choose>
                <xsl:when test="number($MarketValueLocal)">
                  <xsl:value-of select="$MarketValueLocal"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>

            <xsl:variable name="MarketValueLocalBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL27"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValueBase>
              <xsl:choose>
                <xsl:when test="number($MarketValueLocalBase)">
                  <xsl:value-of select="$MarketValueLocalBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValueBase>-->


            <PBSymbol>

              <!--<xsl:value-of select="normalize-space(COL6)"/>-->
				<xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>


          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
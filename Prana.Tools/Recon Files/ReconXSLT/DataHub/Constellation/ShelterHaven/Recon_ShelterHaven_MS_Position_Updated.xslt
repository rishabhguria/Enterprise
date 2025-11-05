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
		<xsl:if test="COL50='PUTL'or COL50='CALLL'">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before($Symbol,'1')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),5,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),3,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),1,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),7,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<!--<xsl:value-of select="format-number(substring(substring-after($Symbol,$UnderlyingSymbol),8,5),'#.00')"/>-->
				<xsl:value-of select="format-number(COL56,'#.00')"/>
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
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
			<!--</xsl:otherwise>-->
			<!--

			</xsl:choose>-->
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

	<xsl:template name="GetSuffix">
		<xsl:param name="Suffix"/>
		<xsl:choose>
			<xsl:when test="$Suffix = 'FP'">
				<xsl:value-of select="'-EEB'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'NA'">
				<xsl:value-of select="'-EEB'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'BZ'">
				<xsl:value-of select="'-BSP'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'HK'">
				<xsl:value-of select="'-HKG'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'PA'">
				<xsl:value-of select="'-EEB'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'L'">
				<xsl:value-of select="'-LON'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'TT'">
				<xsl:value-of select="'-TAI'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'T'">
				<xsl:value-of select="'-TSE'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'BR'">
				<xsl:value-of select="'-EEB'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'OL'">
				<xsl:value-of select="'-OSL'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'J'">
				<xsl:value-of select="'-JSE'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'CN'">
				<xsl:value-of select="'-TC'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'KQ'">
				<xsl:value-of select="'-KOQ'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'FP'">
				<xsl:value-of select="'-EEB'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'LN'">
				<xsl:value-of select="'-LON'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'TT'">
				<xsl:value-of select="'-TAI'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'MI'">
				<xsl:value-of select="'-MIL'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'KS'">
				<xsl:value-of select="'-KOR'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'LN'">
				<xsl:value-of select="'-LON'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'TWO'">
				<xsl:value-of select="'-GTS'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'T'">
				<xsl:value-of select="'-TSE'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'TW'">
				<xsl:value-of select="'-TAI'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'DE'">
				<xsl:value-of select="'-FRA'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'SM'">
				<xsl:value-of select="'-MAC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//Comparision">       
        	

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL28"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="varPBName">
					<xsl:value-of select="'Morgan Stanley and Co. International plc'"/>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Quantity) and contains(COL50,'CASH')!='true'">
				
						<PositionMaster>
							<xsl:variable name = "PB_FUND_NAME">
								<xsl:value-of select="COL4"/>
							</xsl:variable>

							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name= $varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:variable>

							<xsl:variable name="PB_Symbol">
								<xsl:value-of select ="COL6"/>
							</xsl:variable>
							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= $varPBName]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
							</xsl:variable>


							<xsl:variable name="PB_CountnerParty" select="COL5"/>
							<xsl:variable name="PRANA_CounterPartyID">
								<xsl:value-of select="document('../../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= '$varPBName']/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
							</xsl:variable>

							<xsl:variable name="varPBSymbol">
								<xsl:value-of select="COL14"/>
							</xsl:variable>

							<xsl:variable name="varDescription">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name="varNetPosition">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL27"/>
                </xsl:call-template>							
                
							</xsl:variable>

							<!--<xsl:variable name="varCostBasis">
              <xsl:value-of select="COL30"/>
            </xsl:variable>-->

							<xsl:variable name="varFXConversionMethodOperator">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name="varFXRate">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name="varCommission">
								<xsl:value-of select="COL12"/>
							</xsl:variable>

							<xsl:variable name="varFees">
								<xsl:value-of select="0"/>
							</xsl:variable>

							<xsl:variable name="varMiscFees">
								<xsl:value-of select="COL16"/>
							</xsl:variable>

							<xsl:variable name="varClearingFee">
								<xsl:value-of select="COL22"/>
							</xsl:variable>

							<xsl:variable name="varStampDuty">
								<xsl:value-of select="COL23"/>
							</xsl:variable>

							<!--<xsl:variable name="varMarketValue">
				<xsl:value-of select="COL32"/>
			</xsl:variable>-->

							<xsl:variable name="varMarketValue">
								<xsl:choose>
									<xsl:when test="COL51 ='EQUITY SWAP'">
										<xsl:value-of select="COL32"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="COL34"/>
									</xsl:otherwise>
								</xsl:choose>

							</xsl:variable>

							<xsl:variable name="varMarketValueBase">
								<xsl:choose>
									<xsl:when test="COL51 ='EQUITY SWAP'">
										<xsl:value-of select="COL33"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="COL35"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varSuffix">
								<xsl:call-template name="GetSuffix">
									<xsl:with-param name="Suffix" select="substring-after(COL12, '.')"/>
								</xsl:call-template>
							</xsl:variable>

							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<FundName>
										<xsl:value-of select='$PB_FUND_NAME'/>
									</FundName>
								</xsl:when>
								<xsl:otherwise>
									<FundName>
										<xsl:value-of select='$PRANA_FUND_NAME'/>
									</FundName>
								</xsl:otherwise>
							</xsl:choose>
							<NetNotionalValue>
								<xsl:value-of select="0"/>
							</NetNotionalValue>


							<AssetCategory>
								<xsl:choose>

									<xsl:when test='substring-before(COL51, " ") = "Call" or substring-before(COL51, " ") = "Put"'>
										<xsl:value-of select='Option'/>
									</xsl:when>
									<xsl:when test="COL51 ='EQUITY SWAP'">
										<xsl:value-of select="'EquitySwap'"/>
									</xsl:when>
									<xsl:when test="COL51 ='Convertible Bond'">
										<xsl:value-of select="'FixedIncome'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="'Equity'"/>
									</xsl:otherwise>
								</xsl:choose>
							</AssetCategory>
              
              <xsl:variable name="MDate">
                <xsl:value-of select="concat(substring-before(substring-after(substring-after(COL6,' '),'/'),'/'),'/',substring-after(substring-after(substring-after(COL6,'/'),'/'),'/'),'/',substring-before(substring-after(COL6,' '),'/'))"/>
              
              </xsl:variable>


              <xsl:variable name="varFXSymbol">
                <xsl:choose>
                  <xsl:when test="COL50='FX FORWARDS'">
                    <xsl:choose>
                      <xsl:when test="COL8='AUD' or COL8='EUR' or COL8='GBP' or COL8='NZD'">
                        <xsl:value-of select="concat(COL8,'/','USD',' ',$MDate)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat('USD','/',COL8,' ',$MDate)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME !=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>


									<xsl:when test="contains(COL6,'PUT') or contains(COL6,'CALL')">
										<xsl:call-template name="Option">
											<xsl:with-param name="Symbol" select="normalize-space(COL8)"/>
										</xsl:call-template>
									</xsl:when>
                  
            			<xsl:when test="COL50='FX FORWARDS'">
										<xsl:value-of select="$varFXSymbol"/>
									</xsl:when>


									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test='$varSuffix = "" and substring-after(COL14, " ") = "US" and COL52!="EQS"'>
												<xsl:value-of select='substring-before(COL14," ") '/>
											</xsl:when>
											<xsl:when test="COL52='EQS'">
												<xsl:value-of select='concat(substring-before(COL14, " "), $varSuffix,"/SWAP")'/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select='concat(substring-before(COL14, " "), $varSuffix)'/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>


					

							<Side>
								<xsl:choose>
									<xsl:when test="COL50='PUTL'">
										<xsl:choose>
											<xsl:when test="COL29 = 'L'">
												<xsl:value-of select="'Buy to Open'"/>
											</xsl:when>
											<xsl:when test="COL29 = 'S'">
												<xsl:value-of select="'Sell to Open'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>


									<xsl:otherwise>
										<xsl:choose>

											<xsl:when test="COL29 = 'L'">
												<xsl:value-of select="'Buy'"/>
											</xsl:when>
											<xsl:when test="COL29 = 'S'">
												<xsl:value-of select="'Sell short'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>


								</xsl:choose>
							</Side>


              <xsl:variable name="varFXNetPosition">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL27"/>
                </xsl:call-template>

              </xsl:variable>
							<Quantity>
                <xsl:choose>
                  <xsl:when test="COL50='FX FORWARDS'">
                    <xsl:choose>
                      <xsl:when test="COL8='AUD' or COL8='EUR' or COL8='GBP' or COL8='NZD'">
                        <xsl:value-of select="$varNetPosition"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="$varFXNetPosition"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>                
                  
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="number($varNetPosition)">
                        <xsl:value-of select="$varNetPosition"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="0"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>            
								
							</Quantity>


							<MarkPrice>
               <xsl:choose>
									<xsl:when test="COL50='FX FORWARDS'">
										<xsl:value-of select="COL31"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="COL30"/>
									</xsl:otherwise>
								</xsl:choose>
							
							</MarkPrice>

							<CurrencySymbol>
								<xsl:value-of select ="COL44"/>
							</CurrencySymbol>

							<MarketValue>
                   <xsl:choose>
									<xsl:when test="COL50='FX FORWARDS'">
										<xsl:value-of select="COL37"/>
									</xsl:when>
                     <xsl:when test ="boolean(number($varMarketValue))">
										<xsl:value-of select="$varMarketValue"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>								
							</xsl:choose>           
							
							</MarketValue>

							<MarketValueBase>
								<xsl:choose>
									<xsl:when test ="boolean(number($varMarketValueBase))">
										<xsl:value-of select="$varMarketValueBase"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValueBase>

           <xsl:variable name="MaturityDate">
             <xsl:value-of select="concat(substring-before(substring-after(COL55,'-'),'-'),'/',substring-before(COL55,'-'),'/',substring-after(substring-after(COL55,'-'),'-'))"/>           
              </xsl:variable>

              <MaturityDate>
                <xsl:value-of select="$MaturityDate"/>              
              </MaturityDate>



						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>

							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>

							<FundName>
								<xsl:value-of select="''"/>
							</FundName>

							<FXRate>
								<xsl:value-of select="'0'"/>
							</FXRate>

							<Side>
								<xsl:value-of select="''"/>
							</Side>
							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>


							<AvgPX>
								<xsl:value-of select="0"/>
							</AvgPX>

							<NetNotionalValue>
								<xsl:value-of select="0"/>
							</NetNotionalValue>


							<NetNotionalValueBase>
								<xsl:value-of select="0"/>
							</NetNotionalValueBase>


							<MarketValueBase>
								<xsl:value-of select="0"/>
							</MarketValueBase>
							<MarketValue>
								<xsl:value-of select="0"/>
							</MarketValue>

							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>

							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>

							<MarkPriceBase>
								<xsl:value-of select="0"/>
							</MarkPriceBase>
							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>


							<UnitCost>
								<xsl:value-of select="0"/>
							</UnitCost>

               <MaturityDate>
                <xsl:value-of select="''"/>              
              </MaturityDate>

							<CompanyName>
								<xsl:value-of select="''"/>
							</CompanyName>
							<SMRequest>
								<xsl:value-of select="''"/>
							</SMRequest>
						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>

</xsl:stylesheet>

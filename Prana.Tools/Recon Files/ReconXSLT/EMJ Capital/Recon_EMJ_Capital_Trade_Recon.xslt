<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
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




	<xsl:template name="MonthCode">
		<xsl:param name="Month" />
		<xsl:param name="PutOrCall" />
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'A'" />
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'B'" />
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'C'" />
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'D'" />
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'E'" />
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'F'" />
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'G'" />
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'H'" />
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'I'" />
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'J'" />
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'K'" />
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'L'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'M'" />
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'N'" />
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'O'" />
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'P'" />
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'Q'" />
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'R'" />
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'S'" />
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'T'" />
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'U'" />
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'V'" />
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'W'" />
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'X'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>


	<xsl:template name="Option">
		<xsl:param name="varSymbol" />
		<xsl:variable name="var">
			<xsl:value-of select="substring-after($varSymbol,' ')" />
		</xsl:variable>
		<xsl:variable name="UnderlyingSymbol">
			<xsl:value-of select="COL5" />
		</xsl:variable>
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring(normalize-space(COL4),string-length(COL4)-6,2)" />
		</xsl:variable>
		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring(normalize-space(COL4),string-length(COL4)-9,2)" />
		</xsl:variable>


		<xsl:variable name="ExpiryYear">

			<xsl:value-of select="substring(normalize-space(COL4),string-length(COL4)-1,2)" />
		</xsl:variable>
		<xsl:variable name="PutORCall">				
			<xsl:value-of select="substring(COL4,1,1)" />
		</xsl:variable>
		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(substring(substring-after(COL3,' '),9,8)div 1000,'#.00')"/>
		</xsl:variable>

		<xsl:variable name="MonthCodVar">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="$ExpiryMonth" />
				<xsl:with-param name="PutOrCall" select="$PutORCall" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="Day">
			<xsl:choose>
				<xsl:when test="substring($ExpiryDay,1,1)='0'">
					<xsl:value-of select="substring($ExpiryDay,7,2)" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$ExpiryDay" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ', $ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)" />
		
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL7"/>
					</xsl:call-template>
				</xsl:variable>		
					
				<xsl:if test ="number($varQuantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="Jefferies"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL4"/>
						</xsl:variable>
												
						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL3)"/>
						</xsl:variable>	
						
												
						<xsl:variable name="varSymbols">
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>						


						<xsl:variable name="varSEDOL">
							<xsl:value-of select="normalize-space(COL3)" />
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="AssetType">
							<xsl:choose>
								<xsl:when test="contains(COL4,'CALL') or contains(COL4,'PUT')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>																
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$AssetType='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="varSymbol" select="normalize-space(COL3)" />
									</xsl:call-template>
								</xsl:when>								
								<xsl:when test="$varSymbols!='*' or $varSymbols!=''">
									<xsl:value-of select="$varSymbols"/>
								</xsl:when>
								<xsl:when test="$varSEDOL!=''">
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
								<xsl:when test="$varSEDOL!=''">
									<xsl:value-of select="$varSEDOL"/>
								</xsl:when>							
								<xsl:when test="$varSymbols!='*'  or $varSymbols!=''">
									<xsl:value-of select="''" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL16"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>

							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</AccountName>

						<xsl:variable name="varSide" select="COL6"/>

						<Side>
							<xsl:choose>
								<xsl:when test="$AssetType='EquityOption'">
									<xsl:choose>
										<xsl:when test="$varSide='Buy' or $varSide='BY'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="$varSide='Sell' or $varSide='SL'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
									    <xsl:otherwise>
                                          <xsl:value-of select="''"/>
                                        </xsl:otherwise>											
										
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$varSide='Buy' or $varSide='BY'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="$varSide='Sell' or $varSide='SL'">
											<xsl:value-of select="'2'"/>
										</xsl:when>	
									 <xsl:otherwise>
                                        <xsl:value-of select="''"/>
                                     </xsl:otherwise>
									</xsl:choose>
								               
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<Quantity>
							<xsl:choose>
								<xsl:when test="$varQuantity &gt; 0">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:when test="$varQuantity &lt; 0">
									<xsl:value-of select="$varQuantity * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="AvgPrice">
                           <xsl:call-template name="Translate">
                              <xsl:with-param name="Number" select="COL8"/>
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
                              <xsl:value-of select="' '"/>
                          </xsl:otherwise>
                        </xsl:choose>
                        </AvgPX>
						
						<xsl:variable name="varsecFees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL10"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name="varOtherBrokerFees">
                      <xsl:call-template name="Translate">
                        <xsl:with-param name="Number" select="COL11"/>
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
						
						<xsl:variable name="varTotalCommissionandFees">
						<xsl:call-template name="Translate">
                            <xsl:with-param name="Number" select="$Commission + $varsecFees + $varOtherBrokerFees "/>
                          </xsl:call-template>
                       </xsl:variable>
						
							
						<TotalCommissionandFees>
							<xsl:choose>
								<xsl:when test="$varTotalCommissionandFees &gt; 0">
									<xsl:value-of select="$varTotalCommissionandFees"/>
								</xsl:when>	
								<xsl:when test="$varTotalCommissionandFees &lt; 0">
									<xsl:value-of select="$varTotalCommissionandFees * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="' '"/>
								</xsl:otherwise>
							</xsl:choose>
						</TotalCommissionandFees>

						<SecFee>
							<xsl:choose>
								<xsl:when test=" $varsecFees  &gt; 0">
									<xsl:value-of select="$varsecFees"/>
								</xsl:when>
								<xsl:when test=" $varsecFees &lt; 0">
									<xsl:value-of select="$varsecFees * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>

						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="normalize-space(COL14)"/>
						</xsl:variable>				
						
						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/CounterPartyMapping.xml')/CounterPartyMapping/PB[@Name=$PB_NAME]/CounterPartyData[@MappedBrokerCode=$PB_BROKER_NAME]/@BrokerCode"/>
						</xsl:variable>
						
						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_BROKER_ID!='')">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>												

						<xsl:variable name="varPositionStartDate">							
						   <xsl:value-of select ="COL1"/>
						</xsl:variable>

						<TradeDate>
							<xsl:value-of select ="$varPositionStartDate"/>
						</TradeDate>
						
						<xsl:variable name="varSettlementDate">
							 <xsl:value-of select ="COL18"/>				
						</xsl:variable>
						
						<SettlementDate>
							<xsl:value-of select ="$varSettlementDate"/>
						</SettlementDate>
						
                    <OtherBrokerFees>
                      <xsl:choose>
                        <xsl:when test="$varOtherBrokerFees &gt; 0">
                          <xsl:value-of select="$varOtherBrokerFees"/>
                        </xsl:when>
                        <xsl:when test="$varOtherBrokerFees &lt; 0">
                          <xsl:value-of select="$varOtherBrokerFees * (-1)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="' '"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </OtherBrokerFees>
						   
					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
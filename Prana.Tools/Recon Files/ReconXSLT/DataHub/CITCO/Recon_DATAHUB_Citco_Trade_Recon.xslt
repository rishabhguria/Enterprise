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

	<xsl:template name="MonthCodes">
		<xsl:param name="vrMonth"/>
		<xsl:choose>
			<xsl:when test="$vrMonth='Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$vrMonth='Feb'">
				<xsl:value-of select="'2'"/>
			</xsl:when>
			<xsl:when test="$vrMonth='Mar'">
				<xsl:value-of select="'3'"/>
			</xsl:when>
			<xsl:when test="$vrMonth='Apr'">
				<xsl:value-of select="'4'"/>
			</xsl:when>
			<xsl:when test="$vrMonth='May'">
				<xsl:value-of select="'5'"/>
			</xsl:when>
			<xsl:when test="$vrMonth='Jun'">
				<xsl:value-of select="'6'"/>
			</xsl:when>
			<xsl:when test="$vrMonth='Jul'">
				<xsl:value-of select="'7'"/>
			</xsl:when>
			<xsl:when test="$vrMonth='Aug'">
				<xsl:value-of select="'8'"/>
			</xsl:when>
			<xsl:when test="$vrMonth='Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$vrMonth='Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$vrMonth='Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$vrMonth='Dec'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
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
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
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
		<xsl:if test="contains(COL20,'CALL') or contains(COL20,'PUT')">
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

			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
		</xsl:if>
	</xsl:template>



	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL82"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Quantity)">
						<PositionMaster>

							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'CITCO'"/>
							</xsl:variable>

							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:choose>
									<xsl:when test="normalize-space(COL16)='Credit Default Swap' or normalize-space(COL16)='Index Swap'">
										<xsl:value-of select="COL19"/>
									</xsl:when>
									<xsl:when test="normalize-space(COL17)='Future'">
										<xsl:value-of select="COL49"/>
									</xsl:when>
									<xsl:when test="normalize-space(COL17)='Option'">
										<xsl:value-of select="COL19"/>
									</xsl:when>
									<xsl:when test="COL48='*' or COL48=''">
										<xsl:value-of select="COL127"/>
									</xsl:when>
							
									<xsl:otherwise>
										<xsl:value-of select="COL19"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>

							<xsl:variable name="Asset1">
								<xsl:choose>
									<xsl:when test="COL17='Option'">
										<xsl:value-of select="'EquityOption'"/>
									</xsl:when>
									<xsl:when test="COL17='Swap'">
										<xsl:value-of select="'EquitySwap'"/>
									</xsl:when>
									<xsl:when test="COL17='Future'">
										<xsl:value-of select="'Future'"/>
									</xsl:when>
									<xsl:when test="COL17='Bond'">
										<xsl:value-of select="'FixedIncome'"/>
									</xsl:when>

									<xsl:when test="COL17='Equity'">
										<xsl:value-of select="'Equity'"/>
									</xsl:when>
									<xsl:when test="COL16='Credit Default Swap'">
										<xsl:value-of select="'CDX'"/>
									</xsl:when>
									<xsl:when test="COL17='Currency'">
										<xsl:value-of select="'FX'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varUnderlying">
								<xsl:value-of select="substring-before(COL49,' ')"/>
							</xsl:variable>

							<xsl:variable name="varBDate">
								<xsl:value-of select="substring-before(substring-after(substring-after(COL49,' '),' '),' ')"/>
							</xsl:variable>

							<xsl:variable name="varBStrikeAndPutCal">
								<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(COL49,' '),' '),' '),' Equity')"/>
							</xsl:variable>

							<xsl:variable name="VArSymbol" select="COL49"/>

							<!--<For Making the FX Symbol from PB>-->
							<xsl:variable name="varMontH1">
								<xsl:call-template name="MonthCodes">
									<xsl:with-param name="vrMonth" select="substring-before(substring-after(COL38,' '),' ')"/>
								</xsl:call-template>
							</xsl:variable>
							<xsl:variable name="varDay1">
								<xsl:value-of select="substring(COL38,1,2)"/>
							</xsl:variable>
							<xsl:variable name="varYear1">
								<xsl:value-of select="substring(COL38,8,4)"/>
							</xsl:variable>

						
							<xsl:variable name="Symbol">
								<xsl:choose>
									<xsl:when test="COL45='USD' and $Asset1='Equity'">
										<xsl:value-of select="concat(substring-before($VArSymbol,' '),' ','US')"/>
									</xsl:when>
									<xsl:when test="COL45='USD' and $Asset1='EquitySwap'">
										<xsl:value-of select="concat('EQS.',substring-before(COL50,' '))"/>
									</xsl:when>
									<xsl:when test="COL45!='USD' and $Asset1='EquitySwap'">
										<xsl:value-of select="concat('EQS.',COL55)"/>
									</xsl:when>
									<xsl:when test="COL27='USD' and COL16='Equity Option'">
										<xsl:value-of select="concat($varUnderlying,' ',$varBDate,' ',$varBStrikeAndPutCal)"/>
									</xsl:when>

									<xsl:when test="$Asset1='Future'">
										<xsl:value-of select="substring-before($VArSymbol,' ')"/>
									</xsl:when>
									<xsl:when test="$Asset1='FixedIncome'">
										<xsl:value-of select="substring-before($VArSymbol,' ')"/>
									</xsl:when>
																		
									<xsl:when test="$Asset1='FX'">
										<xsl:value-of select="concat(COL143,'-',$varMontH1,'',$varDay1,'',$varYear1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="COL54"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>


							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>
									<xsl:when test="$Symbol!=''">
										<xsl:value-of select="$Symbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>

							</Symbol>

							<!--<IDCOOptionSymbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="$Asset ='EquityOption'">
										<xsl:value-of select="concat(COL34,'U')"/>
									</xsl:when>

									<xsl:when test="$varSEDOL!='*'">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="$Symbol!='*'">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>

							</IDCOOptionSymbol>-->


							<xsl:variable name="PB_FUND_NAME" select="COL11"/>

							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
									<xsl:with-param name="Number" select="COL77"/>
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
									<xsl:with-param name="Number" select="COL85"/>
								</xsl:call-template>
							</xsl:variable>
							<TotalCommissionandFees>
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
							</TotalCommissionandFees>

							<!--<xsl:variable name="FxRate">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL5"/>
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
							</FxRate>-->

							<xsl:variable name="SecFees">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL69"/>
								</xsl:call-template>
							</xsl:variable>
							<SecFees>
								<xsl:choose>
									<xsl:when test="$SecFees &gt; 0">
										<xsl:value-of select="$SecFees"/>
									</xsl:when>
									<xsl:when test="$SecFees &lt; 0">
										<xsl:value-of select="$SecFees * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecFees>

							<xsl:variable name="Date1">
								<xsl:choose>									
										<xsl:when test="contains(COL150,'/')">
											<xsl:value-of select="substring-before(COL150,' ')"/>
										</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="COL150"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							
							<xsl:variable name="var1" select="substring(substring-after(substring-after($Date1,'-'),'-'),1,4)"/>
							<xsl:variable name="var2" select="substring-before(substring-after($Date1,'-'),'-')"/>
							<xsl:variable name="var3" select="substring-before($Date1,'-')"/>

							<xsl:variable name="varMontH">
								<xsl:call-template name="MonthCodes">
									<xsl:with-param name="vrMonth" select="substring-before(substring-after($Date1,' '),' ')"/>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="varDay">
								<xsl:value-of select="substring($Date1,1,2)"/>
							</xsl:variable>

							<xsl:variable name="varYear">
								<xsl:value-of select="substring($Date1,8,4)"/>
							</xsl:variable>
							
							<TradeDate>
								<xsl:choose>
									<xsl:when test ="contains($Date1,'/')">
										<xsl:value-of select="$Date1"/>
									</xsl:when>
									<xsl:when test ="string-length($var3)=4">
										<xsl:value-of select="concat($var2,'/',$var1,'/',$var3)"/>
									</xsl:when>
									<xsl:when test="string-length($var3)=2">
										<xsl:value-of select="concat($var2,'/',$var3,'/',$var1)"/>
									</xsl:when>
									<xsl:when test="contains($Date1,' ')">
										<xsl:value-of select="concat($varMontH,'/',$varDay,'/',$varYear)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</TradeDate>							


			
							<xsl:variable name="Asset">
								<xsl:choose>
									<xsl:when test="COL16='Equity Swap'">
										<xsl:value-of select="'EquitySwap'"/>
									</xsl:when>
									<xsl:when test="COL16='Equity Option'">
										<xsl:value-of select="'EquityOption'"/>
									</xsl:when>
									<xsl:when test="COL16='Credit Default Swap'">
										<xsl:value-of select="'CDX'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'Equity'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varSide">
								<xsl:value-of select="COL21"/>
							</xsl:variable>

							<Side>
								
								<xsl:choose>
									<xsl:when test="$varSide ='Buy' and $Asset1='EquityOption'  and COL33='I'">
										<xsl:value-of select="'Buy to Open'"/>
									</xsl:when>
									<xsl:when test="$varSide ='Sell' and $Asset1='EquityOption' and COL33='I'">
										<xsl:value-of select="'Sell to Open'"/>
									</xsl:when>
									<xsl:when test="$varSide ='Buy' and ($Asset1='EquityOption' or $Asset1='Future') and COL33='L'">
										<xsl:value-of select="'Buy to Close'"/>
									</xsl:when>
									<xsl:when test="$varSide ='Sell' and ($Asset1='EquityOption') and COL33='L'">
										<!--<xsl:when test="$varSide ='Sell' and ($Asset1='EquityOption' or $Asset1='Future') and COL33='L'">-->
										<xsl:value-of select="'Sell to Close'"/>
									</xsl:when>
									<xsl:when test="$varSide ='Sell' and COL33='I'">
										<!--<xsl:when test="$varSide ='Sell' and $Asset1='Future' and COL33='I'">-->
										<xsl:value-of select="'Sell Short'"/>
									</xsl:when>

									<xsl:when test="$varSide ='Buy'">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>

									<xsl:when test="$varSide ='Sell'">
										<xsl:value-of select="'Sell'"/>
									</xsl:when>

									<xsl:when test="$varSide='Short'">
										<xsl:value-of select="'Sell Short'"/>
									</xsl:when>

									<xsl:when test="$varSide ='Cover'">
										<xsl:value-of select="'Buy to Close'"/>
									</xsl:when>

								</xsl:choose>



							</Side>

							<xsl:variable name="Netnotional">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL97"/>
								</xsl:call-template>
							</xsl:variable>
							<NetNotionalValue>
								<xsl:choose>
									<xsl:when test="$Netnotional &gt; 0">
										<xsl:value-of select="$Netnotional"/>
									</xsl:when>
									<xsl:when test="$Netnotional &lt; 0">
										<xsl:value-of select="$Netnotional * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetNotionalValue>

							

							<PBSymbol>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</PBSymbol>

							<CompanyName>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</CompanyName>

							

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

							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>

							<AvgPX>
								<xsl:value-of select="0"/>
							</AvgPX>


							<Commission>
								<xsl:value-of select="0"/>
							</Commission>

							<SecFees>
								<xsl:value-of select="0"/>
							</SecFees>
						
							
							<StampDuty>
								<xsl:value-of select="0"/>
							</StampDuty>


							<NetNotionalValue>
								<xsl:value-of select="0"/>
							</NetNotionalValue>

							<TotalCommissionandFees>
								<xsl:value-of select="''"/>
							</TotalCommissionandFees>

							<Asset>
								<xsl:value-of select="''"/>
							</Asset>

							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>

							<Side>
								<xsl:value-of select="''"/>
							</Side>
						

							<CompanyName>
								<xsl:value-of select="''"/>
							</CompanyName>

							

						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>



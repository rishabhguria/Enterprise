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

	<xsl:template name ="MonthCode">
		<xsl:param name ="varMonth"/>
		<xsl:param name ="varPutCall"/>
		<xsl:choose>
			<xsl:when  test ="$varMonth=1 and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=2 and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=3 and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=4 and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=5 and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=6 and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=7 and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=8 and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=9 and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when  test =" $varMonth=10 and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=11 and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=12  and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=1 and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=2 and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=3 and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=4 and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=5 and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=6 and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=7 and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=8 and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=9 and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=10 and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=11 and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=12 and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="varUnderLying"/>
		<xsl:param name="varEX"/>
		<xsl:param name="varputCall"/>
		<xsl:param name="varStrike"/>
		<xsl:param name="Asset"/>


		<xsl:variable name ="varUnderlyingSymbol">
			<xsl:choose>
				<xsl:when test="$Asset='OPT'">
					<xsl:value-of select ="$varUnderLying"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varYear">
			<xsl:choose>
				<xsl:when test="$Asset='OPT'">
					<xsl:value-of select ="substring(substring-after(substring-after($varEX,'/'),'/'),3,2)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varMonth">
			<xsl:choose>
				<xsl:when test="$Asset='OPT'">
					<xsl:value-of select ="substring-before($varEX,'/')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varExDay">
			<xsl:choose>
				<xsl:when test="$Asset='OPT'">
					<xsl:value-of select ="substring-before(substring-after($varEX,'/'),'/')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varPutCall">
			<xsl:choose>
				<xsl:when test="$Asset='OPT'">
					<xsl:value-of select ="$varputCall"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name ="varStrikePrice">
			<xsl:choose>
				<xsl:when test="$Asset='OPT'">
					<xsl:value-of select ="format-number($varStrike,'#.00')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varMonthCode">
			<xsl:call-template name ="MonthCode">
				<xsl:with-param name ="varMonth" select ="number($varMonth)"/>
				<xsl:with-param name ="varPutCall" select="$varPutCall"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="varDays">
			<xsl:choose>
				<xsl:when test="substring($varExDay,1,1)='0'">
					<xsl:value-of select="substring($varExDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varExDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varThirdFriday">

			<xsl:choose>
				<xsl:when test="$Asset='OPT' and number($varYear) and number($varMonth)">
					<xsl:value-of select='my:Now(concat(20,$varYear),$varMonth)'/>
				</xsl:when>
			</xsl:choose>

		</xsl:variable>


		<xsl:choose>
			<xsl:when test="$Asset='OPT'">
				<xsl:choose>
					<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
						<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
		</xsl:choose>
		<!--</xsl:param>-->
	</xsl:template>
	<xsl:template match="/">

		<DocumentElement>


			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name ="varQuantity">
					<xsl:value-of select ="number(COL27)"/>
				</xsl:variable>

				<xsl:if test ="number($varQuantity)">

					<PositionMaster>
						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'IB'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL7"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_SUFFIX_NAME" select="substring-after(COL16,'.')"/>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="PB_ROOT_NAME">
							<xsl:choose>
								<xsl:when test="string-length(COL6)=4">
								<xsl:value-of select="substring(COL6,1,2)"/>
								</xsl:when>
								<xsl:when test="string-length(COL6)=5">
									<xsl:value-of select="substring(COL6,1,3)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring(COL6,1,2)"/>
								</xsl:otherwise>
							</xsl:choose>
							
							
						</xsl:variable>


						<xsl:variable name ="PRANA_ROOT_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name=$PB_NAME]/SymbolData[@InstrmentCode = $PB_ROOT_NAME]/@UnderlyingCode"/>
						</xsl:variable>

						<xsl:variable name="Underlying">
							<xsl:choose>
								<xsl:when test="$PRANA_ROOT_NAME!=''">
									<xsl:value-of select="$PRANA_ROOT_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_ROOT_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varOption">
							<xsl:call-template name="Option">
								<xsl:with-param name="varUnderLying" select="COL14"/>
								<xsl:with-param name="varEX" select ="COL18"/>
								<xsl:with-param name="varputCall" select="COL19"/>
								<xsl:with-param name="varStrike" select="COL17"/>
								<xsl:with-param name="Asset" select="COL4"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="Asset" select="COL4"/>

						<xsl:variable name="FutureOptionSymbol">
							<xsl:value-of select="translate(COL6,' ','')"/>
						</xsl:variable>


						<!--<xsl:variable name="Underlying2">
							<xsl:choose>
										<xsl:when test="string-length(COL6)=4">
											<xsl:value-of select="concat(substring(COL6,1,2),' ',substring(COL6,3))"/>
										</xsl:when>
										<xsl:when test="string-length(COL6)=5">
											<xsl:value-of select="concat(substring(COL6,1,3),' ',substring(COL6,4))"/>
										</xsl:when>
							</xsl:choose>
						</xsl:variable>-->

						<xsl:variable name="Symbol">
							<xsl:choose>
								<xsl:when test="contains(COL6,' ')">
									<xsl:value-of select="substring-before(COL6,' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(COL6)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<Symbol>

							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='OPT'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='FUT'">
									<xsl:choose>
										<xsl:when test="string-length(COL6)=4">
											<xsl:value-of select="concat($Underlying,' ',substring(COL6,3))"/>
										</xsl:when>
										<xsl:when test="string-length(COL6)=5">
											<xsl:value-of select="concat($Underlying,' ',substring(COL6,4))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat($Underlying,' ',substring(COL6,3))"/>
										</xsl:otherwise>
									</xsl:choose>
							
											
																		
								</xsl:when>


								<xsl:when test="$Asset='FOP'">
									<xsl:value-of select="concat($Underlying,' ',substring($FutureOptionSymbol,3))"/>
								</xsl:when>
								<xsl:when test="$Asset='FSFOP'">
									<xsl:value-of select="concat($Underlying,' ',substring($FutureOptionSymbol,3))"/>
								</xsl:when>

								<xsl:when test="$Asset='STK'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:when test="$Asset='CASH'">
									<xsl:value-of select="concat(substring-before(COL7,'.'),'-',substring-after(COL7,'.'),' ','SPOT')"/>
								</xsl:when>

								<xsl:when test="COL6!='*'">
									<xsl:value-of select="normalize-space(concat(COL6,$PRANA_SUFFIX_NAME))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>



						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$Asset='OPT'">
									<xsl:value-of select="concat(COL6,'U')"/>

								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>



						<ISIN>
							<xsl:value-of select="COL12"/>
						</ISIN>

						<!--<StrikePrice>
							<xsl:value-of select="COL17"/>
						</StrikePrice>-->

						<!--<ExpirationDate>
							<xsl:value-of select="COL18"/>
						</ExpirationDate>

						<PutOrCall>
							<xsl:choose>
								<xsl:when test="COL19='P'">
									<xsl:value-of select="'Put'"/>
								</xsl:when>
								<xsl:when test="COL19='C'">
									<xsl:value-of select="'Call'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</PutOrCall>-->

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
						<xsl:variable name="PB_CountnerParty" select="''"/>
						<xsl:variable name="PRANA_CounterPartyID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'IB']/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
						</xsl:variable>


						<CounterPartyID>
							<!--<xsl:choose>
									<xsl:when test="number($PRANA_CounterPartyID)">
										<xsl:value-of select="$PRANA_CounterPartyID"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>-->
							<xsl:value-of select="'19'"/>
						</CounterPartyID>

						<PositionStartDate>
							<xsl:value-of select="concat(substring(COL22,5,2),'/',substring(COL22,7,2),'/',substring(COL22,1,4))"/>
						</PositionStartDate>

						<NetPosition>
							<xsl:choose>
								<xsl:when  test="number($varQuantity) &gt; 0">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:when test="number($varQuantity) &lt; 0">
									<xsl:value-of select="$varQuantity* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="Underlyer" select="COL14"/>

						<xsl:variable name="Prana_Multiplier">
							<xsl:value-of select ="document('../ReconMappingXML/PriceMulMapping.xml')/PriceMulMapping/PB[@Name=$PB_NAME]/MultiplierData[@PranaRoot=$Underlyer]/@Multiplier"/>
						</xsl:variable>

						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="translate(COL28,',','')"/>
						</xsl:variable>

						<xsl:variable name="Cost">
							<xsl:choose>
								<xsl:when test="number($Prana_Multiplier)">
									<xsl:value-of select="$varAvgPrice div $Prana_Multiplier"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varAvgPrice"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test ='number($Cost) &lt; 0'>
									<xsl:value-of select ="$Cost*-1"/>
								</xsl:when>
								<xsl:when test ='number($Cost) &gt; 0'>
									<xsl:value-of select ='$Cost'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name="varSide" select="COL46"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Asset='OPT'">
									<xsl:choose>
										<xsl:when  test="$varSide='BUY'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when  test="$varSide='SELL'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="$Asset='FUT' or $Asset='CASH'">
									<xsl:choose>
										<xsl:when  test="$varSide='BUY'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when  test="$varSide='SELL'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>	
							
								<xsl:when test="$Asset='FOP' or $Asset='CASH' or $Asset='FSFOP'">
									<xsl:choose>
										<xsl:when  test="$varSide='BUY'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when  test="$varSide='SELL'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>			

								
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when  test="$varSide='BUY' and COL35='O'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when  test="$varSide='SELL' and COL35='O'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:when  test="$varSide='BUY' and COL35='C'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when  test="$varSide='SELL' and COL35='C'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name="varFX">
							<xsl:value-of select="number(COL5)"/>
						</xsl:variable>

						<FXRate>
							<xsl:choose>
								<xsl:when test ="number($varFX)">
									<xsl:value-of select="$varFX"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose >
						</FXRate>

						<xsl:variable name="Commission">
							<xsl:value-of select="translate(COL32,',','')"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test ='number($Commission) &lt; 0'>
									<xsl:value-of select ="$Commission*-1"/>
								</xsl:when>
								<xsl:when test ='number($Commission) &gt; 0'>
									<xsl:value-of select ='$Commission'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="TransactionLevy">
							<xsl:value-of select="translate(COL31,',','')"/>
						</xsl:variable>

						<TransactionLevy>
							<xsl:choose>
								<xsl:when test ='number($TransactionLevy) &lt; 0'>
									<xsl:value-of select ="$TransactionLevy*-1"/>
								</xsl:when>
								<xsl:when test ='number($TransactionLevy) &gt; 0'>
									<xsl:value-of select ='$TransactionLevy'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TransactionLevy>


						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
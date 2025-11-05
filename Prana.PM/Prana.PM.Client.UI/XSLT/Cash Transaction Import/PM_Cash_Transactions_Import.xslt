<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

	<xsl:template name="tempMetalSymbolCode">
		<xsl:param name="paramMetalSymbol"/>
		<!-- 1 characters for metal code -->
		<!--  e.g. A represents A = aluminium-->
		<xsl:choose>
			<xsl:when test ="$paramMetalSymbol='U S DOLLAR'">
				<xsl:value-of select ="'USD'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='UK POUND STERLING'">
				<xsl:value-of select ="'GBP'"/>
			</xsl:when>

			<xsl:when test ="$paramMetalSymbol='EURO'">
				<xsl:value-of select ="'EUR'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='AUSTRALIAN DOLLAR'">
				<xsl:value-of select ="'AUD'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='HONG KONG DOLLAR'">
				<xsl:value-of select ="'HKD'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='INDONESIAN RUPIAH'">
				<xsl:value-of select ="'IDR'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='JAPANESE YEN'">
				<xsl:value-of select ="'JPY'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='MALAYSIAN RINGGIT'">
				<xsl:value-of select ="'MYR'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='NEW ZEALAND DOLLAR'">
				<xsl:value-of select ="'NZD'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='PHILIPPINO PESO'">
				<xsl:value-of select ="'PBP'"/>
			</xsl:when>
			<xsl:when test ="$paramMetalSymbol='SINGAPORE DOLLAR'">
				<xsl:value-of select ="'SGD'"/>
			</xsl:when>

			<xsl:when test ="$paramMetalSymbol='THAI BAHT'">
				<xsl:value-of select ="'THB'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="normalize-space(COL1) != 'Account Name'">
					<PositionMaster>

						<!--Start Of mandatory columns-->

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name = "CashValueType">
							<xsl:choose>
								<xsl:when test="COL7='Interest'">
									<xsl:choose>
										<xsl:when test="COL8 &gt; 0">
											<xsl:value-of select="'Positive'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'Negative'"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Long'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name = "TransactionType">
							<xsl:choose>
								<xsl:when test="COL7='Withdraw' or COL7='Deposit' or COL7='Interest' or COL7='Management Fee' or COL7='Spot Fx' or COL7='Return of Capital' or COL7='SL Rebate/Fee'">
									<xsl:value-of select="COL7"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'other'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="PRANA_CASHTRANSACTION_TYPE">
							<xsl:value-of select="document('../ReconMappingXml/CashTransactionMapping.xml')/CashTransactionMapping/PB[@Name='GS']/CashTransactionMappingData[@PBSubAccountCode=$TransactionType and @LongOrShort=$CashValueType]/@CashTransactionType"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Jefferies']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>


						<FundName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME = ''">
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</FundName>


						<FundID>
							<xsl:value-of select="0"/>
						</FundID>

						<xsl:variable name = "amount" >
							<xsl:choose>
								<xsl:when test="COL7='Withdraw' or COL7='Deposit' or COL7='Spot FX'">
									<!--<xsl:value-of select="COL8"/>-->
									<xsl:choose>
										<xsl:when test="COL8 &gt; 0">
											<xsl:value-of select="COL8"/>
										</xsl:when>
										<xsl:when test="COL8 &lt; 0">
											<xsl:value-of select="COL8*(-1)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<!--<xsl:value-of select="COL32"/>-->
									<xsl:choose>
										<xsl:when test="COL32 &gt; 0">
											<xsl:value-of select="COL32"/>
										</xsl:when>
										<xsl:when test="COL32 &lt; 0">
											<xsl:value-of select="COL32*(-1)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Date>
							<xsl:value-of select="COL2"/>
						</Date>

						<Description>
							<xsl:value-of select ="COL7"/>
						</Description>

						<CurrencyName>
							<xsl:choose>
								<xsl:when test="COL24=''">
									<xsl:value-of select="'USD'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL24"/>
								</xsl:otherwise>
							</xsl:choose>
						</CurrencyName>

						<CurrencyID>
							<xsl:value-of select ="0"/>
						</CurrencyID>
						<CashTransactionType>
							<xsl:value-of select="$PRANA_CASHTRANSACTION_TYPE"/>
						</CashTransactionType>

						<Amount>
							<xsl:value-of select ="$amount"/>
						</Amount>

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
